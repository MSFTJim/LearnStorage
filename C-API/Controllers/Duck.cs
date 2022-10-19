using Microsoft.AspNetCore.Mvc;

namespace C_API.Controllers;

[ApiController]
//[Route("[controller]")]
  [Route("Tim")]
public class Duck : ControllerBase
{
    private readonly ILogger<Duck> _logger;
    public Duck(ILogger<Duck> logger)
    {
        _logger = logger;
    }

    [Route("Truck")]
    //  [HttpGet]  
     
    public string GetDucky()
    {
        return "Ducky!!";

        }

    [HttpGet("GetDucky2")]       
    public string GetDucky2()
    {
        return "Ducky2!!";

        }

    [HttpPost]       
    public string FileUpload(IFormFile file)
    {
        int dog = 0;
        if (file.Length > 0)
            dog++;

        
        return "File Upload Endpoint Success.";

        }
}
