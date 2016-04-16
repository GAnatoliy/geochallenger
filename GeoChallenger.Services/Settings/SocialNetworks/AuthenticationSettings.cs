using System.Collections.Generic;

namespace GeoChallenger.Services.Settings.SocialNetworks
{
    /// <summary>
    ///     Application authentication settings
    /// </summary>
    public class AuthenticationSettings
    {
        /// <summary>
        ///     User oauth token lifetime in days
        /// </summary>
        public int UserTokenLifetimeInDays { get; set; }
    }
}
