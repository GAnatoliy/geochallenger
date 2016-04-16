using System.Web.Http.Filters;
using GeoChallenger.Web.Api.Filters;


namespace GeoChallenger.Web.Api.Config
{
    public class FilterConfig
    {
        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new ExceptionFilter());
        }
    }
}