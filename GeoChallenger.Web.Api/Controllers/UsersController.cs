using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Web.Api.Models.Users;
using Microsoft.AspNet.Identity;

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

        /// <summary>
        ///     Get authenticated user info.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("me")]
        public async Task<UserReadViewModel> GetUserInfoAsync()
        {
            return _mapper.Map<UserReadViewModel>(await _usersService.GetUserAsync(User.Identity.GetUserId<int>()));
        }

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
