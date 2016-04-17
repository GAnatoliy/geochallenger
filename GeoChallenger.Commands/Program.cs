using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using GeoChallenger.Commands.Command;
using GeoChallenger.Commands.Commands;
using NLog;


namespace GeoChallenger.Commands
{
    class Program
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        // NOTE: Only one command is supported at this moement.
        static void Main(string[] args)
        {
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
        }
    }
}
