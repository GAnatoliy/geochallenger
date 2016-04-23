using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GeoChallenger.Web.Api.Models.Pois
{
    /// <summary>
    ///     Types of media that's use in GeoChallenger
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MediaTypeViewModel : byte
    {
        UserAvatarImage = 0,
        PoiImage,
        PoiVideo
    }
}