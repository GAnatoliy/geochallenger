using System.IO;
using System.Threading.Tasks;
using GeoChallenger.Services.Interfaces.DTO.Media;

namespace GeoChallenger.Services.Interfaces
{
    public interface IMediaService
    {
        /// <summary>
        ///     Upload media to the blob
        /// </summary>
        /// <param name="stream">Media stream</param>
        /// <param name="mediaType">Media type</param>
        /// <returns></returns>
        Task<MediaUploadResultDto> UploadAsync(Stream stream, MediaTypeDto mediaType);
    }
}
