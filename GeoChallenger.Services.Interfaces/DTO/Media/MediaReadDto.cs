namespace GeoChallenger.Services.Interfaces.DTO.Media
{
    public class MediaReadDto
    {
        /// <summary>
        ///     Media content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     Media file Url in blob
        /// </summary>
        public string Url { get; set; }
    }
}
