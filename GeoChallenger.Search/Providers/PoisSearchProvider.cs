using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoChallenger.Search.Documents;
using GeoChallenger.Services.Interfaces.DTO;
using Nest;
using NLog;


namespace GeoChallenger.Search.Providers
{
    public class PoisSearchProvider: DocumentsSearchProvider<PoiDocument>, IPoisSearchProvider
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly SearchSettings _searchSettings;

        public PoisSearchProvider(SearchSettings searchSettings) : base(searchSettings)
        {
            _searchSettings = searchSettings;
        }

        public async Task<IList<PoiDocument>> SearchAllAsync(string searchText = null, GeoBoundingBoxDto geoBoundingBox = null)
        {
            var client = GetClient();

            var result = await client.SearchAsync<PoiDocument>(s => s
                // Use filter since for geo location isn't important result order.
                .Query(q => +q.Bool(bs => bs
                    .Must(mq => string.IsNullOrEmpty(searchText) ? mq : mq.MultiMatch(m => m
                        .Query(searchText)
                        .Type(TextQueryType.BestFields)
                        .Fields(f => f.Field(p => p.Title).Field(p => p.Content))
                        .TieBreaker(0.3)
                        .MinimumShouldMatch("75%")),
                        mq => geoBoundingBox == null ? mq : mq.GeoBoundingBox(b => b
                            .BoundingBox(geoBoundingBox.TopLeftLatitude, geoBoundingBox.TopLeftLongitude, geoBoundingBox.BottomRightLatitude, geoBoundingBox.BottomRightLongitude)
                            .Field(p => p.Location))))
                ));
            return result.Documents.ToList();
        }

        public async Task<IList<PoiDocument>> SearchSimilarPoiAsync(int samplePoiId, double samplePoiLatitude, double samplePoiLongitude, int limit)
        {
            const int MAX_QUERY_TERM = 12;
            const int MIN_TEM_FREQUENCY = 1;

            var client = GetClient();

            // NOTE: we have to use index name instead of alias since more-like-this
            // query doesn't recognize index for some reason. 
            // https://github.com/elastic/elasticsearch/issues/14944
            var response = await client.GetIndexAsync(_searchSettings.IndexAlias);
            if (!response.IsValid) {
                throw new Exception($"Can't get index, error {response.ServerError.Error}");
            }

            var index = response.Indices.First();
            var indexName = index.Key;

            var result = await client.SearchAsync<PoiDocument>(s => s
                .Query(q => q
                    .MoreLikeThis(ms => ms
                        .Fields(f => f.Field(fd => fd.Title).Field(fd => fd.Content))
                        .Like(ls => ls.Document(ld => ld.Id(samplePoiId).Index(indexName)))
                        .MaxQueryTerms(MAX_QUERY_TERM)
                        .MinTermFrequency(MIN_TEM_FREQUENCY)) )
                // TODO: rescore by distance.
                // https://www.elastic.co/guide/en/elasticsearch/guide/current/sorting-by-distance.html#scoring-by-distance
                // .Rescore(rs => rs.RescoreQuery(rq => rq.Query(q => q.GeoDistance())))
                .Size(limit));

            _log.Info(Encoding.UTF8.GetString(result.CallDetails.RequestBodyInBytes));
            return result.Documents.ToList();
        }
    }
}