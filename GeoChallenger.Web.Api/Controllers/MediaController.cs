using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Media;
using GeoChallenger.Web.Api.Models.Media;
using GeoChallenger.Web.Api.Models.Pois;

namespace GeoChallenger.Web.Api.Controllers
{
    [RoutePrefix("media")]
    public class MediaController : ApiController
    {
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;

        public MediaController(IMediaService mediaService, IMapper mapper)
        {
            _mediaService = mediaService;
            _mapper = mapper;
        }

        [Route("")]
        public async Task<IHttpActionResult> Get(string url, string size = null)
        {
            return Ok();
        }

        [Route("{mediaType:MediaTypeViewModel}")]
        public async Task<MediaUploadResultViewModel> Post(MediaTypeViewModel mediaType)
        {
            if (!Request.Content.IsMimeMultipartContent()) {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var streamProvider = await Request.Content.ReadAsMultipartAsync();
            var mediaUploadResultViewModel = new MediaUploadResultViewModel();

            foreach (var content in streamProvider.Contents) {
                if (content.Headers.ContentType == null) {
                    continue;
                }

                var mediaUploadResultDto = await _mediaService.UploadAsync(await content.ReadAsStreamAsync(), _mapper.Map<MediaTypeDto>(mediaType));
                mediaUploadResultViewModel.Names.Add(mediaUploadResultDto.Name);
                mediaUploadResultViewModel.MediaLinks.Add(mediaUploadResultDto.Uri);
                mediaUploadResultViewModel.ContentTypes.Add(content.Headers.ContentType.ToString());
            }

            return mediaUploadResultViewModel;
        }
    }
}
