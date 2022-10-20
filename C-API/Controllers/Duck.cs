using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace C_API.Controllers;

 public class Item
            {   public int Id { get; set; }
                public string? Name { get; set; }
            }

  

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

    private Item? myItem { get; set; }

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
    public string FileUpload()
    {
        int dog = 0;
        

    HttpRequest multipartRequest = HttpContext.Request;

    string jsonData = multipartRequest.Form["jsonData"];

    myItem = JsonSerializer.Deserialize<Item>(jsonData);

    

    // if (file.Length > 0)
    dog++;

        
        return "File Upload Endpoint Success.";

        }
}
