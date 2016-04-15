using NUnit.Framework;

namespace GeoChallenger.Web.Api.Test
{
    /// <summary>
    /// Tests mapping configuration.
    /// </summary>
    [TestFixture]
    public class MappingConfigurationTests
    {
        [Test]
        public void TestMappings()
        {
            var configuration = MapperConfig.CreateMapperConfiguration();
            configuration.AssertConfigurationIsValid();
        }
    }
}
