using System.Threading.Tasks;
using GeoChallenger.Services.Interfaces.DTO.Users;

namespace GeoChallenger.Services.Interfaces
{
    public interface IUsersService
    {
        /// <summary>
        ///     Get user by social account uid and social account type
        /// </summary>
        /// <param name="oauthToken">Social network account oauth verification token</param>
        /// <param name="accountTypeDto">Social network account type</param>
        /// <returns>Return user</returns>
        Task<UserDto> GetUserAsync(string oauthToken, AccountTypeDto accountTypeDto);
    }
}
