using Nest;


namespace GeoChallenger.Search.Documents
{
    [ElasticsearchType(Name = "pois")]
    public class PoiDocument: BaseDocument
    {
        public PoiDocument()
        {
            Location = new LocationDocument();
        }

        /// <summary>
        /// POI title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Short preview of the content.
        /// </summary>
        [String(Index = FieldIndexOption.No)]
        public string ContentPreview { get; set; }

        /// <summary>
        /// Text content of the poi.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     POI location address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Location of this poi.
        /// </summary>
        // TODO: use GeoHashPrecision = 7, GeoHashPrefix = true when aggregation will be needed.
        [GeoPoint(LatLon = true)]
        public LocationDocument Location { get; set; }
    }
}
