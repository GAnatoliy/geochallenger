using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO;
using GeoChallenger.Services.Interfaces.DTO.Pois;
using GeoChallenger.Web.Api.Models.Pois;
using Microsoft.AspNet.Identity;


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

        #region GET
        /// <summary>
        /// Search poi based on the location and text.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IList<PoiPreviewViewModel>> GetAsync(string query = null, double? topLeftLatitude = null, double? topLeftLongitude = null,
            double? bottomRightLatitude = null, double? bottomRightLongitude = null)
        {
            GeoBoundingBoxDto boundingBox = (topLeftLatitude.HasValue && topLeftLongitude.HasValue
                && bottomRightLatitude.HasValue && bottomRightLongitude.HasValue) ?
                new GeoBoundingBoxDto(topLeftLatitude.Value, topLeftLongitude.Value, bottomRightLatitude.Value, bottomRightLongitude.Value) :
                null;

            var pois = await _poisService.SearchPoisAsync(query, boundingBox);

            return _mapper.Map<IList<PoiPreviewViewModel>>(pois);
        }

        /// <summary>
        /// GetAsync poi stub by id
        /// </summary>
        /// <param name="poiId">Poi Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{poiId:int}", Name = "GetPoiById")]
        public async Task<PoiReadViewModel> GetAsync(int poiId)
        {
            var poiDto = await _poisService.GetPoiAsync(poiId);
            if (poiDto == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var poiReadViewModel = _mapper.Map<PoiReadViewModel>(poiDto);
            //poiReadViewModel.Media = _mapper.Map<List<PoiMediaReadViewModel>>(await _poisService.GetPoiMediaAsync(poiId));
            await LoadPoiMedia(poiReadViewModel);

            return poiReadViewModel;
        }

        [HttpGet]
        [Route("similar")]
        public async Task<IList<PoiPreviewViewModel>> SearchSimilarPoiAsync(int samplePoiId, int take)
        {
            return _mapper.Map<List<PoiPreviewViewModel>>(
                await _poisService.SearchSimilarPoiAsync(samplePoiId, take));
        }

        [HttpGet]
        [Route("my")]
        [Authorize]
        public async Task<IList<PoiPreviewViewModel>> GetUserPoisAsync()
        {
            return _mapper.Map<IList<PoiPreviewViewModel>>(
                await _poisService.GetUserPoisAsync(User.Identity.GetUserId<int>()));
        }
        [HttpGet]
        [Route("testexception")]
        public async Task TestExceptionAsync()
        {
            throw new Exception("See inner", new Exception("Some exception"));
        }

        #endregion

        #region POST
        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> CreateAsync(PoiUpdateViewModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var createdPoi = await _poisService.CreatePoiAsync(User.Identity.GetUserId<int>(), _mapper.Map<PoiUpdateDto>(model));

            var poiReadViewModel = _mapper.Map<PoiReadViewModel>(createdPoi);
            await LoadPoiMedia(poiReadViewModel);

            return Created(Url.Link("GetPoiById", new { poiId = createdPoi.Id }), poiReadViewModel);
        }

        [HttpPost]
        [Route("{poiId:int}/checkin")]
        [Authorize]
        public async Task<IHttpActionResult> CheckInAsync(int poiId)
        {
            var poi = await _poisService.GetPoiAsync(poiId);
            if (poi == null) {
                return NotFound();
            }

            await _poisService.CheckinPoiAsync(User.Identity.GetUserId<int>(), poiId);

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        #endregion

        #region PUT
        [HttpPut]
        [Route("{poiId:int}")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateAsync(int poiId, PoiUpdateViewModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var poi = await _poisService.GetPoiAsync(poiId);
            if (poi == null) {
                return NotFound();
            }

            var updatedPoi = await _poisService.UpdatePoiAsync(poiId, _mapper.Map<PoiUpdateDto>(model));
            return Ok(updatedPoi);
        }
        #endregion

        #region DELETE
        [HttpDelete]
        [Route("{poiId:int}")]
        [Authorize]
        public async Task<IHttpActionResult> DeleteAsync(int poiId)
        {
            var poi = await _poisService.GetPoiAsync(poiId);
            if (poi == null) {
                return NotFound();
            }

            await _poisService.DeletePoiAsync(User.Identity.GetUserId<int>(), poiId);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }
        #endregion

        #region Private methods

        private async Task LoadPoiMedia(PoiReadViewModel poiReadViewModel)
        {
            poiReadViewModel.Media = _mapper.Map<List<PoiMediaReadViewModel>>(await _poisService.GetPoiMediaAsync(poiReadViewModel.Id));
        }

        #endregion


    }
}
