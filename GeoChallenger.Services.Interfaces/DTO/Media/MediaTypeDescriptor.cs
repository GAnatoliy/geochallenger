namespace GeoChallenger.Services.Interfaces.DTO.Media
{
    /// <summary>
    ///     Describes azure blob container
    /// </summary>
    public class MediaTypeDescriptor
    {
        /// <summary>
        ///     Azure blob container name
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        ///     Azure blob container type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     Azure blob container description
        /// </summary>
        public string FileExtension { get; set; }
    }
}
