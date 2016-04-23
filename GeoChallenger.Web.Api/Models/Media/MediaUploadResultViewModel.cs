using System.Collections.Generic;

namespace GeoChallenger.Web.Api.Models.Media
{
    public class MediaUploadResultViewModel
    {
        public IList<string> Names { get; set; }

        public IList<string> MediaLinks { get; set; }

        public IList<string> ContentTypes { get; set; }
    }
}