using Microsoft.AspNetCore.Mvc;

namespace SocialMemoryEvent.Controllers;

[ApiController]
[Route("[controller]")]
public class SocialMemoryController : ControllerBase
{

    private readonly ILogger<SocialMemoryController> _logger;

    public SocialMemoryController(ILogger<SocialMemoryController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetSocialMemory")]
    public string GetSocialMemory()
    {
        return "SocialMemoryController Get Successful!";
    }

    [HttpPost]
    public string SocialMemoryUpload()
    {
        int dog = 0;


        HttpRequest multipartRequest = HttpContext.Request;

        string jsonData = multipartRequest.Form["jsonData"];

        // myItem = JsonSerializer.Deserialize<Item>(jsonData);

        dog++;


        return "File Upload Endpoint Success.";

    }

}
