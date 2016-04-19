using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeoChallenger.Database;
using GeoChallenger.Domains.Routes;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Routes;
using GeoChallenger.Services.Queries;
using Mehdime.Entity;

namespace GeoChallenger.Services
{
    public class RoutesService : IRoutesService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        public RoutesService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper; 
        }

        public async Task<List<RouteDto>> GetRoutesAsync(int userId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var routes = await dbContextScope.DbContexts.Get<GeoChallengerContext>().Routes
                    .GetRoutes(userId)
                    .ToListAsync();
                await LoadPoisForListAsync(routes);
                return _mapper.Map<List<RouteDto>>(routes);
            }
        }

        public async Task<RouteDto> GetRouteAsync(int userId, int routeId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var route = await dbContextScope.DbContexts.Get<GeoChallengerContext>().Routes
                    .GetRoute(userId, routeId)
                    .SingleOrDefaultAsync();

                await LoadPoisAsync(route);
                return _mapper.Map<RouteDto>(route);
            }
        }

        public async Task UpdateRouteAsync(int routeId, RouteUpdateDto routeUpdateDto)
        {
            /*
            Poi poi;

            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                poi = await context.Pois.FindAsync(poiId);
                if (poi == null || poi.IsDeleted) {
                    throw new ObjectNotFoundException($"Poi with id {poiId} is not found");
                }

                _mapper.Map(poiUpdateDto, poi);

                poi.Content = HtmlHelper.SanitizeHtml(poiUpdateDto.Content);
                poi.ContentPreview = GetContentPreview(poiUpdateDto.Content);

                await dbContextScope.SaveChangesAsync();
            }

            // Update search index.
            try {
                await _poisSearchProvider.IndexAsync(_mapper.Map<PoiDocument>(poi));
            } catch (Exception ex) {
                _log.Warn(ex, $"Can't update search index for poi with id {poi.Id}");
            }

            return _mapper.Map<PoiDto>(poi);            
             */
        }

        private async Task LoadPoisForListAsync(IList<Route> routes)
        {
            using (var dbScopeContext = _dbContextScopeFactory.CreateReadOnly()) {
                var pois = await dbScopeContext.DbContexts.Get<GeoChallengerContext>().Pois
                    .Include(p => p.Routes)
                    .GetPois(routes.Select(r => r.Id).ToList())
                    .ToListAsync();

                foreach (var route in routes) {
                    route.Pois = pois.Where(p => p.Routes.Any(r => !r.IsDeleted && r.Id == route.Id)).ToList();
                }
            }
        }

        private async Task LoadPoisAsync(Route route)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                route.Pois = await dbContextScope.DbContexts.Get<GeoChallengerContext>().Pois
                   .GetPois(route.Id)
                   .ToListAsync();
            }
        }
    }
}