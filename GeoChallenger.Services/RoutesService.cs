using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
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

        public async Task<RouteDto> CreateRouteAsync(int userId, RouteUpdateDto routeUpdateDto)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var route = _mapper.Map<Route>(routeUpdateDto);
                route.Pois = await dbContextScope.DbContexts.Get<GeoChallengerContext>().Pois
                    .GetPois(routeUpdateDto.PoisIds)
                    .ToListAsync();

                route.User = await dbContextScope.DbContexts.Get<GeoChallengerContext>()
                    .Users.FindAsync(userId);

                route.User.Routes.Add(route);
                await dbContextScope.SaveChangesAsync();

                return _mapper.Map<RouteDto>(route);
            }
        }

        public async Task<RouteDto> UpdateRouteAsync(int userId, int routeId, RouteUpdateDto routeUpdateDto)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var route = await context.Routes.GetRoute(userId, routeId)
                    .SingleOrDefaultAsync();

                if (route == null) {
                    throw new ObjectNotFoundException($"Route with id {routeId} is not found");
                }

                _mapper.Map(routeUpdateDto, route);
                route.Pois = await context.Pois.GetPois(routeUpdateDto.PoisIds).ToListAsync();

                await dbContextScope.SaveChangesAsync();
                return _mapper.Map<RouteDto>(route);
            }
        }

        public async Task DeleteRouteAsync(int userId, int routeId)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var route = await context.Routes.GetRoute(userId, routeId)
                    .SingleOrDefaultAsync();

                if (route == null) {
                    throw new ObjectNotFoundException($"Route with id {routeId} is not found");
                }

                route.Delete();
                await context.SaveChangesAsync();
            }
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