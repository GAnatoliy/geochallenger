using System.Collections.Generic;
using GeoChallenger.Services.Interfaces.DTO.Media;

namespace GeoChallenger.Services.Settings.Storage
{
    /// <summary>
    ///     Azure storage settings with containers
    /// </summary>
    public class AzureStorageSettings
    {
        /// <summary>
        ///     Azure storage security key
        /// </summary>
        public string AzureStorageConnectionString { get; set; }

        /// <summary>
        ///     Availables Azure media containers  with settings
        /// </summary>
        public IDictionary<MediaType, MediaTypeDescriptor> MediaContainers { get; set; }
    }
}
