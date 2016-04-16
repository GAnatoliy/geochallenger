using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO;
using GeoChallenger.Web.Api.Models;

namespace GeoChallenger.Web.Api.Controllers
{
    /// <summary>
    ///     Point of Interests controller
    /// </summary>
    [RoutePrefix("api/pois")]
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

        /// <summary>
        ///     Get poi stub by id
        /// </summary>
        /// <param name="poiId">Poi Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{poiId:int}")]
        public async Task<PoiReadViewModel> Get(int poiId)
        {
            var poiDto = await _poisService.GetPoiAsync(poiId);
            if (poiDto == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return _mapper.Map<PoiReadViewModel>(poiDto);
        }

        [HttpPut]
        [Route("{poiId:int}")]
        public async Task<IHttpActionResult> Update(int poiId, PoiUpdateViewModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            await _poisService.UpdatePoiAsync(poiId, _mapper.Map<PoiUpdateDto>(model));
            return Ok();
        }

        [HttpDelete]
        [Route("{poiId:int}")]
        public async Task<IHttpActionResult> Delete(int poiId)
        {
            await _poisService.DeletePoiAsync(poiId);
            return Ok();
        }
    }
}
