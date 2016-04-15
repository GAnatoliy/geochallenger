using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeoChallenger.Services.Interfaces.DTO;


namespace GeoChallenger.Services.Interfaces
{
    /// <summary>
    /// Provide access to the tags.
    /// </summary>
    public interface ITagsService
    {
        /// <summary>
        /// Returns all tags in system.
        /// </summary>
        /// <returns></returns>
        Task<IList<TagDto>> GetTagsAsync();
    }
}
