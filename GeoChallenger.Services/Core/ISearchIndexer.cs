using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GeoChallenger.Search.Documents;
using GeoChallenger.Search.Providers;


namespace GeoChallenger.Services.Core
{
    public interface ISearchIndexer
    {
        Task UpdateSearchIndexAsync<TDomain, TDocument>(Expression<Func<TDomain, bool>> where, Expression<Func<TDomain, int>> id, Func<TDomain, Task<TDocument>> documentFactoryAsync, IDocumentsSearchProvider<TDocument> searchProvider)
            where TDomain : class
            where TDocument : BaseDocument;
    }
}
