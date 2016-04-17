using System;
using System.Threading.Tasks;
using GeoChallenger.Search;
using GeoChallenger.Services.Interfaces;
using NLog;


namespace GeoChallenger.Commands.Commands
{
    public class ReindexCommand
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly ICommands _commands;

        public ReindexCommand(ICommands commands)
        {
            if (commands == null) {
                throw new ArgumentNullException(nameof(commands));
            }
            _commands = commands;
        }

        public async Task RunAsync()
        {
            _log.Info("Reindex command is run.");

            await _commands.ReindexAsync();

            _log.Info("Reindex command is finished.");    
        }
    }
}