using System.Collections.Generic;
using System.Data.Entity;
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
                await LoadPoisForListAsync(routes, dbContextScope);
                return _mapper.Map<List<RouteDto>>(routes);
            }
        }

        private async Task LoadPoisForListAsync(IList<Route> routes, IDbContextReadOnlyScope dbContextScope)
        {
            // TODO: Refactor roughly solution
            foreach (var route in routes) {
                await LoadPoisAsync(route, dbContextScope);
            }

        }

        private async Task LoadPoisAsync(Route route, IDbContextReadOnlyScope dbContextScope)
        {
            route.Pois = await dbContextScope.DbContexts.Get<GeoChallengerContext>().Pois
                .GetPois(route.Id)
                .ToListAsync();
        }
    }
}
