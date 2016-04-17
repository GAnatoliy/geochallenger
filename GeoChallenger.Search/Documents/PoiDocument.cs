using Nest;


namespace GeoChallenger.Search.Documents
{
    [ElasticsearchType(Name = "poi")]
    public class PoiDocument
    {
        /// <summary>
        /// POI Id
        /// </summary>
        public int Id { get; set; }

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
    }
}
