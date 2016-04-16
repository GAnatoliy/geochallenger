using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Web.Api.Models;

namespace GeoChallenger.Web.Api.Controllers
{
    /// <summary>
    ///     Geo tags controller
    /// </summary>
    [RoutePrefix("api/tags")]
    public class PoisController : ApiController
    {
        private readonly IPoisService _poisService;
        private readonly IMapper _mapper;

        public PoisController(IPoisService poisService, IMapper mapper)
        {
            if (poisService == null) {
                throw new ArgumentNullException(nameof(poisService));
            }
            _poisService = poisService;

            if (mapper == null) {
                throw new ArgumentNullException(nameof(mapper));
            }
            _mapper = mapper;
        }

        /// <summary>
        ///     Get all stub pois
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IList<PoiReadViewModel>> Get()
        {
            return _mapper.Map<IList<PoiReadViewModel>>(await _poisService.GetPoisAsync());
        }
    }
}
