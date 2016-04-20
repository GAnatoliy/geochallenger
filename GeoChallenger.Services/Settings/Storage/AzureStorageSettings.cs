using System.Collections.Generic;
using GeoChallenger.Services.Interfaces.DTO.Media;

namespace GeoChallenger.Services.Settings.Storage
{
    public class AzureStorageSettings
    {
        public IDictionary<ImageType, string> ImageContainersName { get; set; }

        public IDictionary<VideoType, string> videoContainersName { get; set; }
    }
}
