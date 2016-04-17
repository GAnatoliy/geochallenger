using System.Collections.Generic;
using System.Threading.Tasks;


namespace GeoChallenger.Services.Interfaces
{
    /// <summary>
    /// Contains commands that isn't related to the concrete services.
    /// </summary>
    // TODO: consider make refactoring and move it to the specified place.
    public interface ICommands
    {
        /// <summary>
        /// Reindex documents in the search engine.
        /// </summary>
        Task ReindexAsync();
    }
}