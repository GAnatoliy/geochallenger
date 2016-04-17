using System;
using System.Threading.Tasks;
using GeoChallenger.Search;
using GeoChallenger.Services.Interfaces;


namespace GeoChallenger.Services
{
    public class Commands: ICommands
    {
        private readonly ISearchConfigurationManager _searchConfigurationManager;
        private readonly IPoisService _poisService;

        public Commands(ISearchConfigurationManager searchConfigurationManager, IPoisService poisService)
        {
            if (searchConfigurationManager == null) {
                throw new ArgumentNullException(nameof(searchConfigurationManager));
            }
            if (poisService == null) {
                throw new ArgumentNullException(nameof(poisService));
            }
            _searchConfigurationManager = searchConfigurationManager;
            _poisService = poisService;
        }

        public async Task ReindexAsync()
        {
            await _searchConfigurationManager.IncreaseIndexVersionAsync();
            await _poisService.UpdatePoisSearchIndexAsync();
        }
    }
}