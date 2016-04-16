using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GeoChallenger.Web.Api.Models.Users
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AccountTypeViewModel : byte
    {
        Google = 0,
        Facebook
    }
}