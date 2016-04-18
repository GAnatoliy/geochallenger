using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GeoChallenger.Database;
using GeoChallenger.Search.Documents;
using GeoChallenger.Search.Providers;
using Mehdime.Entity;
using NLog;

namespace GeoChallenger.Services.Core
{
    public class SearchIndexer : ISearchIndexer
    {
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        public SearchIndexer(IDbContextScopeFactory dbContextScopeFactory)
        {
            if (dbContextScopeFactory == null) {
                throw new ArgumentNullException(nameof(dbContextScopeFactory));
            }
            _dbContextScopeFactory = dbContextScopeFactory;
        }

        // TODO: write unittests.
        public async Task UpdateSearchIndexAsync<TDomain, TDocument>(
            Expression<Func<TDomain, bool>> where,
            Expression<Func<TDomain, int>> order,
            Func<TDomain, Task<TDocument>> documentFactoryAsync,
            IDocumentsSearchProvider<TDocument> searchProvider
            )
            where TDomain: class
            where TDocument: BaseDocument
        {
            const int PAGE_SIZE = 1000;

            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var documentCounts = await context.Set<TDomain>()
                    .Where(where)
                    .CountAsync();

                for (int i = 0; i < documentCounts; i = i + PAGE_SIZE) {
                    var domains = await context.Set<TDomain>()
                        .Where(where)
                        .OrderBy(order)
                        .Skip(i)
                        .Take(PAGE_SIZE)
                        .ToListAsync();

                    // Maps teams document.
                    // NOTE: we can't map list to list since data will be overridden.
                    var documents = new List<TDocument>();
                    foreach (var domain in domains) {
                        documents.Add(await documentFactoryAsync(domain));
                    }

                    await searchProvider.IndexAsync(documents);
                }

                _log.Info($"Indexed {documentCounts} documents of type {typeof(TDocument)}");
            }
        }
    }
}