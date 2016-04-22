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
        ///     Uploaded container name
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        ///     Uploaded file Uri
        /// </summary>
        public string Uri { get; set; }
    }
}
