using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetVips;


namespace azstor.Pages;

public class ControllerAPIModel : PageModel
{
    private HttpClient APIclient = new HttpClient();

    // private readonly IConfiguration _configuration;

    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;
    private readonly long _fileSizeLimit;

    public ControllerAPIModel(ILogger<ControllerAPIModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _fileSizeLimit = _configuration.GetValue<long>("FileSizeLimit");
      
    }

    public string? myAPIMessage { get; set; }
    //public string apiBase { get; set; } = "http://127.0.0.1:5059/";
    public string apiBase { get; set; } = "http://127.0.0.1:5026/";
    public string? apiUrl { get; set; }

    public class Item
    {
        public string? Name { get; set; }
        public string? Desc { get; set; }       
        public DateTime? EventDate { get; set; }

    }
  
    
    public Item thisItem = new();

    [BindProperty]
    public string? APIRoute { get; set; }

    [BindProperty]
    public IFormFile Upload { get; set; } = null!;

    public void OnGet()
    {
        APIRoute = "Cocktail";
        thisItem.Name = "Hello";

    }

    public async Task<IActionResult> OnPostAsync()
    {
        var ms = new MemoryStream();

        if ((Upload.Length > 0) && (Upload.Length < _fileSizeLimit))
        {
            await Upload.CopyToAsync(ms);
            myAPIMessage = "Process was good";
        }
        else
        {
            myAPIMessage = "File size invalid";
        }

        // START of new process to just pass file to API

        apiUrl = apiBase + APIRoute;

        var myHttpRequest = new HttpRequestMessage(HttpMethod.Post, apiUrl);

        var myMultipartFormData = new MultipartFormDataContent();
        ms.Position = 0;
        var myStreamContent = new StreamContent(ms);

        myStreamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(Upload.ContentType);  //Upload.ContentType = "image/jpg"

        myMultipartFormData.Add(myStreamContent, "file", Upload.FileName);

        Item myItem = new();
        myItem.Desc = "";
        myItem.Name = "Notary";

        

        var jsonItem = JsonSerializer.Serialize(myItem);
        // var httpContent = new StringContent(jsonItem, Encoding.UTF8, "application/json");

        myMultipartFormData.Add(new StringContent(jsonItem, Encoding.UTF8, "application/json"), name: "jsonData");


        var response1 = await APIclient.PostAsync(apiUrl, myMultipartFormData);


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

