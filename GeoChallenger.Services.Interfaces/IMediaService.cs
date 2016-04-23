using System.IO;
using System.Threading.Tasks;
using GeoChallenger.Services.Interfaces.DTO.Media;

namespace GeoChallenger.Services.Interfaces
{
    public interface IMediaService
    {
        /// <summary>
        ///     Get blob by blob filename
        /// </summary>
        /// <param name="filename">Blob filename</param>
        /// <param name="mediaType">Blob media type</param>
        /// <returns></returns>
        Task<MediaReadDto> GetBlobUrl(string filename, MediaTypeDto mediaType);

        /// <summary>
        ///     Upload media to the blob
        /// </summary>
        /// <param name="stream">Media stream</param>
        /// <param name="mediaType">Media type</param>
        /// <returns></returns>
        Task<MediaUploadResultDto> UploadAsync(Stream stream, MediaTypeDto mediaType);
    }
}
