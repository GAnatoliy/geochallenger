using System;
using System.Threading.Tasks;
using GeoChallenger.Domains.Users;
using GeoChallenger.Services.Providers.DTO;
using GeoChallenger.Services.Providers.Interfaces;
using GeoChallenger.Services.Settings.SocialNetworks;
using Newtonsoft.Json;

namespace GeoChallenger.Services.Providers.SocialNetworks
{
    public class FacebookProvider : ISocialNetworksProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly FacebookSettings _facebookSettings;

        public FacebookProvider(IHttpProvider httpProvider, FacebookSettings facebookSettings)
        {
            _httpProvider = httpProvider;
            _facebookSettings = facebookSettings;
        }

        public async Task<SocialNetworkValidationData> ValidateCredentialsAsync(string oauthToken)
        {
            var facebookUserData = await GetUserFacebookDataAsync(oauthToken);
            return new SocialNetworkValidationData {
                Uid = facebookUserData.FacebookId,
                Email = facebookUserData.Email,
                Type = AccountType.Facebook
            };
        }

        #region Private methods

        /// <summary>
        ///     Get user facebook data by oauth token
        /// </summary>
        /// <param name="facebookOAuthToken">Facebook oauth token</param>
        /// <returns></returns>
        private async Task<FacebookUserData> GetUserFacebookDataAsync(string facebookOAuthToken)
        {
            var facebookUserData = await _httpProvider.HttpGetRequestAsync<FacebookUserData>($"{ _facebookSettings.FacebookVerificationUrl }{facebookOAuthToken}");
            
            if (facebookUserData == null) {
                throw new Exception("Invalid facebook token.");
            }

            if (string.IsNullOrEmpty(facebookUserData.Email)) {
                throw new Exception("Can't recieve user's email from facebook.");
            }

            facebookUserData.Email = facebookUserData.Email.ToLower();
            return facebookUserData;
        }

        /// <summary>
        ///     DTO for data recieved from facebook 
        /// </summary>
        [JsonObject]
        private class FacebookUserData
        {
            /// <summary>
            ///     Facebook user unique Id
            /// </summary>
            [JsonProperty("id")]
            public string FacebookId { get; set; }

            /// <summary>
            ///     User email on Facebook
            ///     By this field merging facebook account with other same email accounts into one
            /// </summary>
            [JsonProperty("email")]
            public string Email { get; set; }
        }

        #endregion
    }
}
