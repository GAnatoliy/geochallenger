using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using GeoChallenger.Commands.Commands;
using GeoChallenger.Commands.Config;
using NLog;
using NLog.Fluent;


namespace GeoChallenger.Commands
{
    class Program
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        // NOTE: Only one command is supported at this moement.
        static void Main(string[] args)
        {
            _log.Info("Start GeoChallenger.Commands");
            try {
                var mapperConfiguration = MapperConfig.CreateMapperConfiguration();
                var container = DIConfig.RegisterDI(mapperConfiguration);

                // All components have single instance scope.
                using (var scope = container.BeginLifetimeScope()) {
                    var command = scope.Resolve<ReindexCommand>();
                    command.RunAsync().Wait();
                }
            } catch (Exception ex) {
                _log.Error(ex, "Command is failed.");
            }
            _log.Info("End GeoChallenger.Commands");
        }
    }
}
