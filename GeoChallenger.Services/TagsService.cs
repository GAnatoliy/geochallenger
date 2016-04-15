using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GeoChallenger.Domains;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO;


namespace GeoChallenger.Services
{
    public class TagsService: ITagsService
    {
        private readonly IMapper _mapper;

        public TagsService(IMapper mapper)
        {
            if (mapper == null) {
                throw new ArgumentNullException(nameof(mapper));
            }
            _mapper = mapper;
        }

        public async Task<IList<TagDto>> GetTagsAsync()
        {
            var tags = new List<Tag> { new Tag { Title = "First"} };
            return _mapper.Map<IList<TagDto>>(tags);
        }
    }
}
