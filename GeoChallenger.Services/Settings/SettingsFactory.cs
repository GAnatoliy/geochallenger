using System.Collections.Generic;
using System.Configuration;
using GeoChallenger.Domains.Media;
using GeoChallenger.Services.Interfaces.DTO.Media;
using GeoChallenger.Services.Settings.SocialNetworks;
using GeoChallenger.Services.Settings.Storage;

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
                FacebookVerificationUrl = "https://graph.facebook.com/me?fields=id,first_name,last_name,name,picture,email&access_token="
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

        /// <summary>
        ///     Azure storage settings factory
        /// </summary>
        /// <returns></returns>
        public static AzureStorageSettings GetAzureStorageSettings()
        {
            return new AzureStorageSettings {
                AzureStorageConnectionString = ConfigurationManager.ConnectionStrings["GeoChallengerStorageConnection"].ConnectionString,
                MediaContainers = new Dictionary<MediaType, MediaTypeDescriptor> {
                    {
                        MediaType.UserAvatarImage, new MediaTypeDescriptor {
                            ContainerName = $"{nameof(MediaType.UserAvatarImage).ToLower()}",
                            ContentType = "image/jpeg",
                            FileExtension = "jpg"
                        }
                    },
                    {
                        MediaType.PoiImage, new MediaTypeDescriptor {
                            ContainerName = $"{nameof(MediaType.PoiImage).ToLower()}",
                            ContentType = "image/jpeg",
                            FileExtension = "jpg"
                        }
                    },
                                        {
                        MediaType.PoiVideo, new MediaTypeDescriptor {
                            ContainerName = $"{nameof(MediaType.PoiVideo).ToLower()}",
                            ContentType = "video/mp4",
                            FileExtension = "mp4"
                        }
                    }
                }
            };
        }
    }
}
