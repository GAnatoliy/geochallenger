using System;
using System.Threading.Tasks;
using GeoChallenger.Domains.Users;
using GeoChallenger.Services.Providers.DTO;
using GeoChallenger.Services.Providers.Interfaces;
using GeoChallenger.Services.Settings.SocialNetworks;
using Newtonsoft.Json;

namespace GeoChallenger.Services.Providers.SocialNetworks
{
    public class GoogleProvider : ISocialNetworksProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly GoogleSettings _googleSettings;

        public GoogleProvider(IHttpProvider httpProvider, GoogleSettings googleSettings)
        {
            _httpProvider = httpProvider;
            _googleSettings = googleSettings;
        }

        public async Task<SocialNetworkValidationData> ValidateCredentialsAsync(string oauthToken)
        {
            var googleUserData = await GetUserGoogleDataAsync(oauthToken);
            return new SocialNetworkValidationData {
                Uid = googleUserData.GoogleId,
                Email = googleUserData.Email,
                Name = googleUserData.Name,
                Type = AccountType.Google
            };
        }

        #region Private methods

        /// <summary>
        ///     Get user google data by oauth token
        /// </summary>
        /// <param name="googleOAuthToken">Google oauth token</param>
        /// <returns></returns>
        private async Task<GoogleUserData> GetUserGoogleDataAsync(string googleOAuthToken)
        {
            var googleUserData = await _httpProvider.HttpGetRequestAsync<GoogleUserData>($"{ _googleSettings.GoogleVerificationUrl }{ googleOAuthToken }");

            if (googleUserData == null) {
                throw new Exception("Invalid google token.");
            }
            if (string.IsNullOrEmpty(googleUserData.Email)) {
                throw new Exception("Can't recieve user's email from google.");
            }

            googleUserData.Email = googleUserData.Email.ToLower();
            return googleUserData;
        }

        /// <summary>
        ///     Facebook user unique Id
        /// </summary>
        [JsonObject]
        private class GoogleUserData
        {
            /// <summary>
            ///     Google user unique Id
            /// </summary>
            [JsonProperty("id")]
            public string GoogleId { get; set; }

            /// <summary>
            ///     User email on Google
            ///     By this field merging google account with other same email accounts into one
            /// </summary>
            [JsonProperty("email")]
            public string Email { get; set; }

            /// <summary>
            ///     User name on Google
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        #endregion
    }
}
