using System.Collections.Generic;
using GeoChallenger.Services.Interfaces.DTO.Media;

namespace GeoChallenger.Services.Settings.Storage
{
    public class AzureStorageSettings
    {
        public IDictionary<ImageTypeDto, string> ImageContainersName { get; set; }

        public IDictionary<VideoTypeDto, string> videoContainersName { get; set; }
    }
}
