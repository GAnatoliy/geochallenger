namespace GeoChallenger.Services.Interfaces.DTO
{
    /// <summary>
    /// Contains information about geo bounding box.
    /// </summary>
    public class GeoBoundingBoxDto
    {
        public GeoBoundingBoxDto(double topLeftLatitude, double topLeftLongitude, double bottomRightLatitude, double bottomRightLongitude)
        {
            TopLeftLatitude = topLeftLatitude;
            TopLeftLongitude = topLeftLongitude;
            BottomRightLatitude = bottomRightLatitude;
            BottomRightLongitude = bottomRightLongitude;
        }

        public double TopLeftLatitude { get; }
        public double TopLeftLongitude { get; }
        public double BottomRightLatitude { get; }
        public double BottomRightLongitude { get; }
    }
}