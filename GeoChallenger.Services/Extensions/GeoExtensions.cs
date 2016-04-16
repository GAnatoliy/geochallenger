using System.Data.Entity.Spatial;
using System.Globalization;

namespace GeoChallenger.Services.Extensions
{
    public static class GeoExtensions
    {
        // 4326 is most common coordinate system used by GPS/Maps
        const int CoorindateSystemId = 4326;

        /// <summary>
        ///     Create DbGeography location point from latitude and Longitude coordinates
        /// </summary>
        /// <param name="latitude">Location latitude</param>
        /// <param name="longitude">Location Longitude</param>
        /// <returns>Return DbGeography location point</returns>
        public static DbGeography CreateLocationPoint(double latitude, double longitude)
        {
            var text = string.Format(CultureInfo.InvariantCulture.NumberFormat, "POINT({0} {1})", longitude, latitude);
            return DbGeography.PointFromText(text, CoorindateSystemId);
        }
    }
}
