using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoChallenger.Search.Providers.Interfaces
{
    /// <summary>
    /// Base interface for document search.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDocumentsSearchProvider<in T>
    {
        /// <summary>
        /// Index documents (create or update)
        /// </summary>
        Task<bool> IndexAsync(T document);

        /// <summary>
        /// Index documents (create or update)
        /// </summary>
        /// <returns>Returns true if no error is occurred.</returns>
        Task<bool> IndexAsync(IEnumerable<T> documents);

        /// <summary>
        /// Delete story.
        /// </summary>
        /// <returns>True if delete was success.</returns>
        Task<bool> DeleteAsync(int documentId);

        /// <summary>
        /// Delete documents.
        /// </summary>
        /// <param name="documentsIds"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(IList<int> documentsIds);
    }
}