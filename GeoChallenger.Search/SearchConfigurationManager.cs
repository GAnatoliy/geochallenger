using System;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using GeoChallenger.Search.Documents;
using Nest;
using NLog;


namespace GeoChallenger.Search
{
    public static class ResponseExtensions
    {
        public static string GetErrorMessage(this IResponse response)
        {
            return response.ServerError?.Error.ToString() ?? response.OriginalException?.Message + response.OriginalException;
        }
    }

    public class SearchConfigurationManager : ISearchConfigurationManager
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly SearchSettings _searchSettings;

        public SearchConfigurationManager(SearchSettings searchSettings)
        {
            if (searchSettings == null) {
                throw new ArgumentNullException(nameof(searchSettings));
            }
            _searchSettings = searchSettings;
        }

        public async Task IncreaseIndexVersionAsync()
        {
            var client = GetClient();

            var response = await client.GetIndexAsync(_searchSettings.IndexAlias);
            if (!response.IsValid) {
                throw new Exception($"Can't get index, error {response.GetErrorMessage()}");
            }

            var index = response.Indices.First();
            var indexName = index.Key;
            var indexSettings = index.Value;

            _log.Info("Start create new index version, old name is '{0}'", indexName);

            // Creates new indexes.
            var newIndexName = GenerateNewIndexName(indexName);

            // Prevent copy mappings, since they should be generated from start.
            // TODO: test this part (create incorrect mapping and check that it is removed after increase index version).
            indexSettings.Mappings = new Mappings();

            // Prevent copy aliases.
            indexSettings.Aliases = new Aliases();

            var createIndexResponse = await client.CreateIndexAsync(newIndexName,
                s => s.InitializeUsing(indexSettings));

            if (!createIndexResponse.IsValid) {
                var message = $"Can't create new index version, old name '{indexName}', new name '{newIndexName}', error details: {createIndexResponse.GetErrorMessage()}";
                throw new Exception(message);
            }

            var mapResponse = await client.MapAsync<PoiDocument>(s => s.AutoMap().Index(newIndexName));
            if (!mapResponse.IsValid) {
                throw new Exception($"Can't create mapping for type {typeof (PoiDocument)}, error '{mapResponse.GetErrorMessage()}");
            }

            var removeAliasResponse = await client.AliasAsync(
                s => s.Remove(r => r.Alias(_searchSettings.IndexAlias).Index(indexName)));
            if (!removeAliasResponse.IsValid) {
                throw new Exception(
                    $"Can't delete alias '{_searchSettings.IndexAlias}' for index '{indexName}', error '{removeAliasResponse.GetErrorMessage()}'");
            }

            var aliasCreationResponse = await client.AliasAsync(
                s => s.Add(addSelector => addSelector.Index(newIndexName).Alias(_searchSettings.IndexAlias)));
            if (!aliasCreationResponse.IsValid) {
                throw new Exception(
                    $"Can't add alias '{_searchSettings.IndexAlias}' for index '{newIndexName}', error '{removeAliasResponse.GetErrorMessage()}'");
            }

            // Delete old index.
            var deleteIndexResponse = client.DeleteIndex(indexName);
            if (!deleteIndexResponse.IsValid) {
                throw new Exception(
                    $"Can't delete old index index '{indexName}', error '{removeAliasResponse.GetErrorMessage()}'");
            }

            _log.Info("End create new index version, old index is '{0}' index, new index is '{1}'", indexName, newIndexName);
        }

        /// <summary>
        /// Open client connection.
        /// </summary>
        private ElasticClient GetClient()
        {
            var node = new Uri(_searchSettings.ElasticSearchHost);
            var connectionPool = new SingleNodeConnectionPool(node);
            var settings = new ConnectionSettings(connectionPool);
            settings.PingTimeout(new TimeSpan(0, 0, 0, 0, _searchSettings.PingTimeout));
            settings.MaximumRetries(_searchSettings.MaximumRetries);
            settings.DefaultIndex(_searchSettings.IndexAlias);

            // TODO: Decide if it is ok to create client for each call.
            return new ElasticClient(settings);
        }

        /// <summary>
        /// Generates new index name by increasing version.
        /// </summary>
        private string GenerateNewIndexName(string oldIndexName)
        {
            const string VERSION_PART = "_v";

            if (!oldIndexName.Contains(VERSION_PART)) {
                throw new Exception(
                    $"Incorrect index name format, it should contain version part e.g. _v1, current name '{oldIndexName}'");
            }

            var separatorIndex = oldIndexName.LastIndexOf("_", StringComparison.Ordinal);
            var name = oldIndexName.Substring(0, separatorIndex);
            var currentVersion = int.Parse(oldIndexName.Substring(separatorIndex + 2));
            var newVersion = currentVersion + 1;
            return name + VERSION_PART + newVersion;
        }
    }
}