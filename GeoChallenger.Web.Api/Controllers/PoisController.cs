using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO;
using GeoChallenger.Services.Interfaces.DTO.Pois;
using GeoChallenger.Web.Api.Models;
using GeoChallenger.Web.Api.Models.Pois;

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
        /// Search poi based on the location and text.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IList<PoiPreviewViewModel>> Get(string query)
        {
            return _mapper.Map<IList<PoiPreviewViewModel>>(await _poisService.SearchPoisAsync(query));
        }

        /// <summary>
        /// Get poi stub by id
        /// </summary>
        /// <param name="poiId">Poi Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{poiId:int}", Name = "GetPoiById")]
        public async Task<PoiReadViewModel> Get(int poiId)
        {
            var poiDto = await _poisService.GetPoiAsync(poiId);
            if (poiDto == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return _mapper.Map<PoiReadViewModel>(poiDto);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create(PoiUpdateViewModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var createdPoi = await _poisService.CreatePoiAsync(_mapper.Map<PoiUpdateDto>(model));

            return Created(Url.Link("GetPoiById", new { poiId = createdPoi.Id }), createdPoi);
        }

        [HttpPut]
        [Route("{poiId:int}")]
        public async Task<IHttpActionResult> Update(int poiId, PoiUpdateViewModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var poi = await _poisService.GetPoiAsync(poiId);
            if (poi == null) {
                var createdPoi = await _poisService.CreatePoiAsync(_mapper.Map<PoiUpdateDto>(model));
                return Created(Url.Link("GetPoiById", new { poiId = createdPoi.Id }), createdPoi);
            } else {
                var updatedPoi = await _poisService.UpdatePoiAsync(poiId, _mapper.Map<PoiUpdateDto>(model));
                return Ok(updatedPoi);
            }
        }

        [HttpDelete]
        [Route("{poiId:int}")]
        public async Task<IHttpActionResult> Delete(int poiId)
        {
            var poi = await _poisService.GetPoiAsync(poiId);
            if (poi == null) {
                return NotFound();
            }

            await _poisService.DeletePoiAsync(poiId);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        [HttpGet]
        [Route("testexception")]
        public async Task TestExceptionAsync()
        {
            throw new Exception("See inner", new Exception("Some exception"));
        }
    }
}
