using System;
using System.Threading.Tasks;
using GeoChallenger.Search;
using NLog;


namespace GeoChallenger.Commands.Commands
{
    public class ReindexCommand
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly ISearchConfigurationManager _searchConfigurationManager;

        public ReindexCommand(ISearchConfigurationManager searchConfigurationManager)
        {
            if (searchConfigurationManager == null) {
                throw new ArgumentNullException(nameof(searchConfigurationManager));
            }
            _searchConfigurationManager = searchConfigurationManager;
        }

        public async Task RunAsync()
        {
            _log.Info("Reindex command is run.");

            await _searchConfigurationManager.IncreaseIndexVersionAsync();

            _log.Info("Reindex command is finished.");    
        }
    }
}