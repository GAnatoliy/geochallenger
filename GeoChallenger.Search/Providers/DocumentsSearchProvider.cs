using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using NLog;


namespace GeoChallenger.Search.Providers
{
    /// <summary>
    /// Base class with common methods for search provider.
    /// </summary>
    public abstract class DocumentsSearchProvider<T>: IDocumentsSearchProvider<T> where T: class
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly SearchSettings _searchSettings;

        protected DocumentsSearchProvider()
        {
            _searchSettings = new SearchSettings();
        }

        public async Task<bool> IndexAsync(T document)
        {
            var result = await GetClient().IndexAsync(document);
            return result.Created;
        }

        public async Task<bool> IndexAsync(IEnumerable<T> documents)
        {
            var documentsList = documents.ToList();

            _log.Info($"Start index documents for type {typeof(T)}");

            if (documentsList.Count == 0) {
                return true;
            }

            var client = GetClient();
            var result = await client.BulkAsync(b => b.IndexMany(documentsList));

            if (result.Errors) {
                result.ItemsWithErrors.ToList().ForEach(i => _log.Warn("Index failed for documents id: '{0}' index: '{1}' error: {2}",
                    i.Id, i.Index, i.Error));
            }

            _log.Info($"End index documents for type {typeof(T)}");

            return !result.Errors;
        }

        public async Task<bool> DeleteAsync(int documentId)
        {
            var result = await GetClient().DeleteAsync<T>(documentId);
            if (!result.Found) {
                _log.Warn("Can't delete search index for type '{0}' with id '{1}', index '{2}'",
                    result.Type, documentId, result.Index);
            }

            return result.Found;
        }

        public async Task<bool> DeleteAsync(IList<int> documentsIds)
        {
            if (documentsIds.Count == 0) {
                return true;
            }

            var found = true;

            foreach (var documentId in documentsIds) {
                var result = await GetClient().DeleteAsync<T>(documentId);
                if (result.Found) {
                    continue;
                }

                _log.Warn(
                    "Can't delete search index for type '{0}' with id '{1}', index '{2}'",
                    result.Type, documentId, result.Index);

                found = result.Found;
            }

            // TODO: consider to use delete by query (we had some issue with this query before, probably
            // it sin't relevant more.
            //var result = await client.DeleteByQueryAsync<T>(delete => delete
            //    .Index(index)
            //    .Query(q => q.Terms(field => field.DocumentId, documents)
            //        ));

            return found;
        }

        /// <summary>
        /// Open client connection.
        /// </summary>
        protected ElasticClient GetClient()
        {
            var node = new Uri(_searchSettings.ElasticSearchHost);
            var connectionPool = new SniffingConnectionPool(new[] { node });
            var settings = new ConnectionSettings(connectionPool);
            settings.PingTimeout(new TimeSpan(0, 0, 0, 0, _searchSettings.PingTimeout));
            settings.MaximumRetries(_searchSettings.MaximumRetries);
            settings.DefaultIndex(_searchSettings.IndexAlias);

            // TODO: Decide if it is ok to create client for each call.
            return new ElasticClient(settings);
        }
    }
}