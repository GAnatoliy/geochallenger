using NLog;


namespace GeoChallenger.Commands.Commands
{
    public class ReindexCommand
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public ReindexCommand()
        {
            
        }

        public void Run()
        {
            _log.Info("Reindex command is finished.");    
        }
    }
}