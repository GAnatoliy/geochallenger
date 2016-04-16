using System.Collections.Generic;
using System.Configuration;
using GeoChallenger.Services.Settings.SocialNetworks;

namespace GeoChallenger.Services.Settings
{
    /// <summary>
    ///     Application settings factory
    /// </summary>
    public static class SettingsFactory
    {
        /// <summary>
        ///     Facebook provider settings factory
        /// </summary>
        /// <returns></returns>
        public static AuthenticationSettings GetAuthenticationSettings()
        {
            return new AuthenticationSettings {
                UserTokenLifetimeInDays = int.Parse(ConfigurationManager.AppSettings["UserTokenLifetimeInDays"])
            };
        }

        /// <summary>
        ///     Facebook provider settings factory
        /// </summary>
        /// <returns></returns>
        public static FacebookSettings GetFacebookSettings()
        {
            return new FacebookSettings {
                FacebookVerificationUrl = "https://graph.facebook.com/me?fields=id,first_name,last_name,picture,email&access_token="
            };
        }

        /// <summary>
        ///     Google provider settings factory
        /// </summary>
        /// <returns></returns>
        public static GoogleSettings GetGoogleSettings()
        {
            return new GoogleSettings {
                GoogleVerificationUrl = "https://www.googleapis.com/oauth2/v1/userinfo?access_token="
            };
        }
    }
}
