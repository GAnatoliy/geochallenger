using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GeoChallenger.Web.Api.Models;

namespace GeoChallenger.Web.Api.Controllers
{
    /// <summary>
    ///     Geo tags controller
    /// </summary>
    [RoutePrefix("api/[controller]")]
    public class TagsController : ApiController
    {
        /// <summary>
        ///     Get all stub tags
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<List<TagViewModel>> Get()
        {
            return new List<TagViewModel> {
                new TagViewModel { Title = "Stub tag" }
            };
        }
    }
}
