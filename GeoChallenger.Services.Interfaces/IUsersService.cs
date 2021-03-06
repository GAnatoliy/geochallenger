﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GeoChallenger.Services.Interfaces.DTO.Users;

namespace GeoChallenger.Services.Interfaces
{
    public interface IUsersService
    {

        #region Queries
        /// <summary>
        /// Get user by social account uid and social account type
        /// </summary>
        /// <param name="oauthToken">Social network account oauth verification token</param>
        /// <param name="accountTypeDto">Social network account type</param>
        /// <returns>Return user</returns>
        Task<UserDto> GetOrGetWithCreatingUserAsync(string oauthToken, AccountTypeDto accountTypeDto);

        /// <summary>
        /// Get user by user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserDto> GetUserAsync(int userId);

        /// <summary>
        /// Get user by social account uid and social account type
        /// </summary>
        /// <param name="accountUid">Social network account uid</param>
        /// <param name="accountTypeDto">Social network account type</param>
        /// <returns></returns>
        Task<UserDto> GetUserAsync(string accountUid, AccountTypeDto accountTypeDto);

        /// <summary>
        /// Returns users with top stories.
        /// </summary>
        /// <returns></returns>
        Task<IList<UserDto>> GetLeaderboardAsync(int take);

        #endregion
    }
}
