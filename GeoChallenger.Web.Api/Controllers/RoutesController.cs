using System;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
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

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetRoutesAsync()
        {
            var routes = await _routesService.GetRoutesAsync(User.Identity.GetUserId<int>());
            return Ok();
        }

        public async Task<IHttpActionResult> Get

    }
}