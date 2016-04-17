using System.Configuration;
using NLog;


namespace GeoChallenger.Search
{
    public class SearchSettings
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private const int DEFAULT_PING_TIMEOUT = 1000;
        private const int DEFAULT_MAXIMUM_RETRIES = 0;

        public SearchSettings()
        {
            ElasticSearchHost = ConfigurationManager.AppSettings["Search.ElasticSearchHost"];
            IndexAlias = ConfigurationManager.AppSettings["Search.IndexAlias"];

            bool isParsed;
            int pingTimeout;
            isParsed = int.TryParse(ConfigurationManager.AppSettings["Search.PingTimeout"], out pingTimeout);
            if (isParsed) {
                PingTimeout = pingTimeout;
            } else {
                PingTimeout = DEFAULT_PING_TIMEOUT;
                _log.Warn("Can't parse setting 'Search.PingTimeout', default value is used.");
            }

            int maximumRetries;
            isParsed = int.TryParse(ConfigurationManager.AppSettings["Search.MaximumRetries"], out maximumRetries);
            if (isParsed) {
                MaximumRetries = maximumRetries;
            } else {
                MaximumRetries = DEFAULT_MAXIMUM_RETRIES;
                _log.Warn("Can't parse setting 'Search.MaximumRetries', default value is used.");
            }
        }

        public string ElasticSearchHost { get; set; }

        public string IndexAlias { get; set; }

        /// <summary>
        /// Ping timeout in miliseconds.
        /// </summary>
        public int PingTimeout { get; set; }

        public int MaximumRetries { get; set; }
    }
}