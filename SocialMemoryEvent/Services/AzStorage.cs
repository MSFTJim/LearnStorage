
using System.Configuration;

namespace SocialMemoryEvent.Services
{
    public class AzStorage
    {
        // private readonly ILogger<AzStorage> _logger;
        private readonly IConfiguration _config;

        // public AzStorage(ILogger<AzStorage> logger, IConfiguration config)
        public AzStorage(IConfiguration config)
        {
            // _logger = logger;
            _config = config;

        }

        public string AzStorageTestMethod()
        {
            return "This is from AzStorageTestMethod";

        }

    } // end class

}
