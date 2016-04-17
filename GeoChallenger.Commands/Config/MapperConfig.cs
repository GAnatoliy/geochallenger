using AutoMapper;


namespace GeoChallenger.Commands.Command
{
    public class MapperConfig
    {
        public static MapperConfiguration CreateMapperConfiguration()
        {
            // TODO: consider to sue profile in order to configure mapping from different places,
            // ex. http://stackoverflow.com/questions/35187475/autofac-and-automapper-new-api-configurationstore-is-gone
            return new MapperConfiguration(Services.MapperConfig.ConfigureMappings);
        } 
    }
}