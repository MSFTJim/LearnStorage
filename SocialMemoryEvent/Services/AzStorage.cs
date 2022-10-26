
using System.Configuration;

namespace SocialMemoryEvent.Services
{
    public class AzStorage
    {
        private readonly ILogger<AzStorage> _logger;
        //private readonly ILogger _logger;
        private readonly IConfiguration _config;

       public AzStorage(ILogger<AzStorage> logger, IConfiguration config)
       // public AzStorage(IConfiguration config)
        {
            _logger = logger;
            _config = config;

        }

        public string AzStorageTestMethod()
        {
             _logger.LogInformation("AzStorageTestMethod  {DT}", 
            DateTime.UtcNow.ToLongTimeString());
            return "This is from AzStorageTestMethod";

        }

    } // end class

}
