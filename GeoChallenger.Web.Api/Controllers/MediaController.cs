using System;
using System.Collections.Generic;
using System.IO;
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
        
        [Route("{mediaType}/{filename}", Name = "GetMedia")]
        public async Task<IHttpActionResult> Get(MediaTypeViewModel mediaType, string filename)
        {
            var mediaReadDto = await _mediaService.GetBlobUrl(filename, _mapper.Map<MediaTypeDto>(mediaType));
            if (mediaReadDto == null) {
                return NotFound();
            }

            return Redirect(mediaReadDto.Url);
        }

        [Route("{mediaType}/{size}/{filename}")]
        public async Task<IHttpActionResult> Get(string mediaType, string size, string filename)
        {
            return Redirect(Url.Link("GetMedia", new {mediaType = mediaType, filename = filename}));
        }

        [Route("{mediaType}")]
        public async Task<IList<MediaUploadResultViewModel>> Post(MediaTypeViewModel mediaType)
        {
            if (!Request.Content.IsMimeMultipartContent()) {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var streamProvider = await Request.Content.ReadAsMultipartAsync();
            var result = new List<MediaUploadResultViewModel>();

            foreach (var content in streamProvider.Contents) {
                if (content.Headers.ContentType == null) {
                    continue;
                }
                result.Add(_mapper.Map<MediaUploadResultViewModel>(await _mediaService.UploadAsync(await content.ReadAsStreamAsync(), _mapper.Map<MediaTypeDto>(mediaType))));
            }

            return result;
        }

        private string ConvertBlobAbsoluteUrl(string fileName, MediaTypeDto mediaType)
        {
            return $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}{Url.Request.RequestUri.LocalPath}/{fileName}/";
        }
    }
}
