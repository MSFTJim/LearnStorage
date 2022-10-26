using Microsoft.AspNetCore.Mvc;
using SocialMemoryEvent.Services;

namespace SocialMemoryEvent.Controllers;

[ApiController]
[Route("[controller]")]
public class SocialMemoryController : ControllerBase
{

    private readonly ILogger<SocialMemoryController> _logger;
    private readonly IConfiguration _config;

    public SocialMemoryController(ILogger<SocialMemoryController> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
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

        TestMethod();

        return "File Upload Endpoint Success.";

    }

    public void TestMethod()
    {
        int dog=0;
        AzStorage Cosmo = new(_config);
 
        
        dog++;


    }

}
