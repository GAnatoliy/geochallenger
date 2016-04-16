using GeoChallenger.Domains.Users;

namespace GeoChallenger.Services.Providers.DTO
{
    /// <summary>
    ///     Social account verification data
    /// </summary>
    public class SocialNetworkValidationData
    {
        /// <summary>
        ///     Social network unique identificator
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        ///     Social network linked email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Social network type
        /// </summary>
        public AccountType Type { get; set; }
    }
}
