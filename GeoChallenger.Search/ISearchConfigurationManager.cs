using System.Collections.Generic;
using System.Threading.Tasks;


namespace GeoChallenger.Search
{
    public interface ISearchConfigurationManager
    {
        /// <summary>
        /// Increase indexes version and change mappings.
        /// </summary>
        Task IncreaseIndexVersionAsync();
    }
}
