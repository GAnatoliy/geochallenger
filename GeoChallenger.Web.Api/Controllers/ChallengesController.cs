using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Challenges;
using GeoChallenger.Services.Interfaces.DTO.Pois;
using GeoChallenger.Web.Api.Models.Challenges;
using GeoChallenger.Web.Api.Models.Pois;
using Microsoft.AspNet.Identity;
using GeoChallenger.Web.Api.Models;

namespace GeoChallenger.Web.Api.Controllers
{
    /// <summary>
    /// Controller that provides access to the challenges.
    /// </summary>
    [RoutePrefix("api/challenges")]
    public class ChallengesController: ApiController
    {
        private readonly IChallengesService _challengesService;
        private readonly IPoisService _poisService;
        private readonly IMapper _mapper;

        public ChallengesController(IChallengesService challengesService, IMapper mapper, IPoisService poisService)
        {
            if (challengesService == null) {
                throw new ArgumentNullException(nameof(challengesService));
            }
            if (mapper == null) {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (poisService == null) {
                throw new ArgumentNullException(nameof(poisService));
            }
            _challengesService = challengesService;
            _mapper = mapper;
            _poisService = poisService;
        }

        #region GET
        /// <summary>
        /// Get challenge by id.
        /// </summary>
        [HttpGet]
        [Route("{challengeId:int}", Name = "GetChallengeById")]
        public async Task<ChallengeReadViewModel> GetAsync(int challengeId)
        {
            var challengeDto = await _challengesService.GetChallengeAsync(challengeId);
            if (challengeDto == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return _mapper.Map<ChallengeReadViewModel>(challengeDto);
        }

        /// <summary>
        /// Returns challenges created by specified user.
        /// </summary>
        [HttpGet]
        [Route("my")]
        [Authorize]
        public async Task<IList<ChallengeReadViewModel>> GetUserChallengesAsync()
        {
            return _mapper.Map<List<ChallengeReadViewModel>>(
               await _challengesService.GetChallengesCreatedByUserAsync(User.Identity.GetUserId<int>()));
        }

        /// <summary>
        /// Returns challenges related to this poi.
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IList<ChallengeReadViewModel>> GetPoiChallengesAsync(int poiId)
        {
            var poiDto = await _poisService.GetPoiAsync(poiId);
            if (poiDto == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return _mapper.Map<List<ChallengeReadViewModel>>(
                await _challengesService.GetChallengesForPoiAsync(poiId));
        }
        #endregion

        #region POST

        /// <summary>
        /// Answer to challenge.
        /// 
        /// User can't answer to own challenge.
        /// </summary>
        /// <param name="challengeId">Id of the challenged.</param>
        /// <param name="answer">User's answer.</param>
        /// <returns>True if answer is correct and false in other case.</returns>
        [HttpPost]
        [Route("{challengeId:int}/answer")]
        [Authorize]
        public async Task<ValueViewModel<bool>> AnswerToChallengeAsync(int challengeId, string answer)
        {
            var challengeDto = await _challengesService.GetChallengeAsync(challengeId);
            if (challengeDto == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var isCorrect = await _challengesService.AnswerToChallengeAsync(User.Identity.GetUserId<int>(), challengeId, answer);

            return new ValueViewModel<bool>(isCorrect);
        }

        /// <summary>
        /// Create new challenge for specified poi.
        /// </summary>
        /// <param name="poiId">Id of the poi where chaelenge will be created.</param>
        /// <param name="model">Challenge's data.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> CreateAsync(int poiId, ChallengeUpdateViewModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var poiDto = await _poisService.GetPoiAsync(poiId);
            if (poiDto == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var createdChallenge = await _challengesService.CreateChallengeAsync(
                User.Identity.GetUserId<int>(), poiId, _mapper.Map<ChallengeUpdateDto>(model));

            return Created(Url.Link("GetChallengeById", new { challengeId = createdChallenge.Id }), createdChallenge);
        }
        #endregion

        #region PUT
        /// <summary>
        /// Update challenge.
        /// 
        /// Only owner can update challenge.
        /// </summary>
        /// <param name="challengeId">Id of the challenge that should be updated.</param>
        /// <param name="model">challenge's data.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{challengeId:int}")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateAsync(int challengeId, ChallengeUpdateViewModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var challengeDto = await _challengesService.GetChallengeAsync(challengeId);
            if (challengeDto == null) {
                return NotFound();
            }

            var updatedChallenge = await _challengesService.UpdateChallengeAsync(User.Identity.GetUserId<int>(), challengeId, 
                _mapper.Map<ChallengeUpdateDto>(model));
          
            return Ok(updatedChallenge);
        }
        #endregion

        #region DELETE

        /// <summary>
        /// Delete specified challenge.
        /// 
        /// Only owner can remove challenge.
        /// </summary>
        /// <param name="challengeId">Id of the challenge that should be removed.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{challengeId:int}")]
        [Authorize]
        public async Task<IHttpActionResult> DeleteAsync(int challengeId)
        {
            var challengeDto = await _challengesService.GetChallengeAsync(challengeId);
            if (challengeDto == null) {
                return NotFound();
            }

            await _challengesService.DeleteChallengeAsync(User.Identity.GetUserId<int>(), challengeId);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }
        #endregion
    }
}