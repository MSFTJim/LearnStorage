using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetVips;


namespace azstor.Pages;

public class ImageAPIModel : PageModel
{
    private HttpClient APIclient = new HttpClient();

    // private readonly IConfiguration _configuration;

    private readonly ILogger<PrivacyModel> _logger;

    public ImageAPIModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public string? myAPIMessage { get; set; }
    public string apiBase { get; set; } = "http://127.0.0.1:5136/";
    public string? apiUrl { get; set; }

    [BindProperty]
    public string? APIRoute { get; set; }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        // var apiUrl = "http://127.0.0.1:5136/";
        // apiUrl = "http://localhost:5136/";

        apiUrl = apiBase + APIRoute;                
        // apiUrl = apiUrl + "weatherforcast";                

        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        var response1 = await APIclient.SendAsync(request);
        

        switch (response1.StatusCode)
        {
            case System.Net.HttpStatusCode.OK:
                myAPIMessage = await response1.Content.ReadAsStringAsync();
                break;
            case System.Net.HttpStatusCode.NoContent:
                myAPIMessage = "No content";
                break;
            case System.Net.HttpStatusCode.NotFound:
                myAPIMessage = "API Route not found!";
                break;
            case System.Net.HttpStatusCode.Forbidden:
                myAPIMessage = "Your Access to this API route is Forbidden!";
                break;
            case System.Net.HttpStatusCode.Unauthorized:
                myAPIMessage = "Your Access to this API route is Unauthorized!";
                break;
            default:
                myAPIMessage = "Unhandled Error!";
                break;

        }

        return Page();

    }

}

