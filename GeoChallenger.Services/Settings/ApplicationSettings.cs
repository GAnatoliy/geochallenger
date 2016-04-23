namespace GeoChallenger.Services.Settings
{
    /// <summary>
    ///     Application authentication settings
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        ///     User oauth token lifetime in days
        /// </summary>
        public int UserTokenLifetimeInDays { get; set; }

        /// <summary>
        ///     Server url
        /// </summary>
        public string ServerUrl { get; set; }
    }
}
