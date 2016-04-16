using System.Threading.Tasks;
using GeoChallenger.Services.Providers.DTO;

namespace GeoChallenger.Services.Providers.Interfaces
{
    /// <summary>
    ///     Social network provider
    /// </summary>
    public interface ISocialNetworksProvider
    {
        /// <summary>
        ///     Validate social network credentials
        /// </summary>
        /// <param name="oauthToken">User social network oauth token</param>
        /// <returns></returns>
        Task<SocialNetworkValidationData> ValidateCredentialsAsync(string oauthToken);
    }
}
