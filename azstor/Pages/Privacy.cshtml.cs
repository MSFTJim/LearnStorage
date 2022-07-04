using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetVips;


namespace azstor.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {


        // using Image image = Image.Thumbnail("image.jpg", 300, 300);
        // image.WriteToFile("my-thumbnail.jpg");


        // using Image image = Image.Thumbnail("owl.jpg", 128, crop: Enums.Interesting.Attention);
        // image.WriteToFile("tn_owl.jpg");

    }
}

