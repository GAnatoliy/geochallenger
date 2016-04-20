using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Web.Api.Models.Users;


namespace GeoChallenger.Web.Api.Controllers
{
    /// <summary>
    /// Provides information about users.
    /// </summary>
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        const int DEFAULT_LEADERBOARD_SIZE = 20;

        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public UsersController(IUsersService usersService, IMapper mapper)
        {
            if (usersService == null) {
                throw new ArgumentNullException(nameof(usersService));
            }
            if (mapper == null) {
                throw new ArgumentNullException(nameof(mapper));
            }
            _usersService = usersService;
            _mapper = mapper;
        }

        #region GET

        [HttpGet]
        [Route("top")]
        public async Task<IList<UserReadViewModel>> GetLeaderboardAsync(int take = DEFAULT_LEADERBOARD_SIZE)
        {
            return _mapper.Map<List<UserReadViewModel>>(
                await _usersService.GetLeaderboardAsync(take));
        } 

        #endregion
    }
}
