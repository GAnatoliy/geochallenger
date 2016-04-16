using System.Threading.Tasks;

namespace GeoChallenger.Services.Providers.Interfaces
{
    public interface IHttpProvider
    {
        /// <summary>
        ///     Create Http GET request to url
        /// </summary>
        /// <typeparam name="TResult">Expected result class</typeparam>
        /// <param name="queryUrl">Query Url with parameters</param>
        /// <returns>Return deserialized response from request</returns>
        Task<TResult> HttpGetRequestAsync<TResult>(string queryUrl) where TResult : class;
    }
}