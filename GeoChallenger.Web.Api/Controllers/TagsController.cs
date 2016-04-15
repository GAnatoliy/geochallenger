using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO;
using GeoChallenger.Web.Api.Models;


namespace GeoChallenger.Web.Api.Controllers
{
    public class TagsController : ApiController
    {
        private readonly ITagsService _tagsService;
        private readonly IMapper _mapper;

        public TagsController(ITagsService tagsService, IMapper mapper)
        {
            if (tagsService == null) {
                throw new ArgumentNullException(nameof(tagsService));
            }
            if (mapper == null) {
                throw new ArgumentNullException(nameof(mapper));
            }
            _tagsService = tagsService;
            _mapper = mapper;
        }

        // GET api/values
        public async Task<IEnumerable<TagViewModel>> Get()
        {
            var tags = await _tagsService.GetTagsAsync();
            return _mapper.Map<IEnumerable<TagViewModel>>(tags);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
