namespace GeoChallenger.Services.Interfaces.DTO.Media
{
    /// <summary>
    ///     Carry azure blob upload result
    /// </summary>
    public class MediaUploadResultDto
    {
        /// <summary>
        ///     Uploaded file name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Uploaded content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     Uploaded media type
        /// </summary>
        public MediaTypeDto MediaType { get; set; }

        /// <summary>
        ///     Uploaded file Url
        /// </summary>
        public string Url { get; set; }
    }
}
