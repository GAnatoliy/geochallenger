using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Routes;
using GeoChallenger.Web.Api.Models.Routes;
using Microsoft.AspNet.Identity;

namespace GeoChallenger.Web.Api.Controllers
{
    [RoutePrefix("api/routes")]
    public class RoutesController : ApiController
    {
        private readonly IRoutesService _routesService;
        private readonly IMapper _mapper;

        public RoutesController(IRoutesService routesService, IMapper mapper)
        {
            if (routesService == null) {
                throw new ArgumentNullException(nameof(routesService));
            }
            _routesService = routesService;

            if (mapper == null) {
                throw new ArgumentNullException(nameof(mapper));
            }
            _mapper = mapper;
        }

        /// <summary>
        /// Get user routes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IList<RouteReadViewModel>> GetRoutesAsync()
        {
            return _mapper.Map<IList<RouteReadViewModel>>(await _routesService.GetRoutesAsync(User.Identity.GetUserId<int>()));
        }

        /// <summary>
        /// Get user route
        /// </summary>
        /// <param name="routeId">Route Id</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("{routeId:int}", Name = "GetRouteById")]
        public async Task<RouteReadViewModel> GetRouteAsync(int routeId)
        {
            return _mapper.Map<RouteReadViewModel>(await _routesService.GetRouteAsync(User.Identity.GetUserId<int>(), routeId));
        }

        /// <summary>
        /// Create new user route
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateRouteAsync(RouteUpdateViewModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var createdRoute = await _routesService.CreateRouteAsync(User.Identity.GetUserId<int>(), _mapper.Map<RouteUpdateDto>(model));
            var createdRouteViewModel = _mapper.Map<RouteReadViewModel>(createdRoute);

            return Created(Url.Link("GetRouteById", new { routeId = createdRoute.Id }), createdRouteViewModel);
        }

        /// <summary>
        /// Update user's route
        /// </summary>
        /// <param name="routeId">Route Id</param>
        /// <param name="model">Updated route information</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("{routeId:int}")]
        public async Task<IHttpActionResult> EditRouteAsync(int routeId, RouteUpdateViewModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(_mapper.Map<RouteReadViewModel>(await _routesService.UpdateRouteAsync(User.Identity.GetUserId<int>(), routeId, _mapper.Map<RouteUpdateDto>(model))));
        }

        /// <summary>
        ///     Update user's route
        /// </summary>
        /// <param name="routeId">Route Id</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("{routeId:int}")]
        public async Task<IHttpActionResult> DeleteRouteAsync(int routeId)
        {
            await _routesService.DeleteRouteAsync(User.Identity.GetUserId<int>(), routeId);
            return Ok();
        }
    }
}