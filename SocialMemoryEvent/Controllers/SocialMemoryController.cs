using Microsoft.AspNetCore.Mvc;
using SocialMemoryEvent.Services;

namespace SocialMemoryEvent.Controllers;

[ApiController]
[Route("[controller]")]
public class SocialMemoryController : ControllerBase
{

    private readonly ILogger<SocialMemoryController> _logger;
    private readonly ILogger<AzStorage> _AzStoragelogger;
    private readonly IConfiguration _config;

    public SocialMemoryController(ILogger<SocialMemoryController> logger, ILogger<AzStorage> logger2, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        _AzStoragelogger = logger2;
    }

    [HttpGet(Name = "GetSocialMemory")]
    public string GetSocialMemory()
    {
        return "SocialMemoryController Get Successful!";
    }

    [HttpPost]
    public string SocialMemoryUpload()
    {
        HttpRequest multipartRequest = HttpContext.Request;

        string jsonData = multipartRequest.Form["jsonData"];

        // myItem = JsonSerializer.Deserialize<Item>(jsonData);

        _logger.LogInformation("SocialMemoryUpload  {DT}", 
            DateTime.UtcNow.ToLongTimeString());
        LocalTestMethod();

        return "File Upload Endpoint Success.";

    }

    public void LocalTestMethod()
    {
        string dog="";
        AzStorage Cosmo = new(_AzStoragelogger, _config);
        dog = Cosmo.AzStorageTestMethod(); 
        
        


    }

}
