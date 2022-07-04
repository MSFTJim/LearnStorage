using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using NetVips;

namespace azstor.Pages;


public class azstorageModel : PageModel
{
    private readonly IConfiguration _configuration;

    public azstorageModel(IConfiguration configuration)
    {
        _configuration = configuration;

    }
    [BindProperty]
    public string? FormData { get; set; }
    // public string FormData2 { get; set; }

    [BindProperty]
    public IFormFile Upload { get; set; } = null!;
    public IFormFile? Upload2 { get; set; }


    public void OnGet()
    {
        int dog = 0;
        dog++;

    }

    public async Task OnPostAsync()
    {
        var ms = new MemoryStream();
        var ms_t = new MemoryStream();
        Boolean OverWrite = true;

        string StorageConnectionString = _configuration["AZURE_STORAGE_CONNECTION_STRING"];
        string CocktailImageContainer = _configuration["CocktailImageContainer"];
        string fileName = "cocktail" + Guid.NewGuid().ToString() + ".jpg";
        //fileName = "cocktail" + ".jpg";

        BlobContainerClient containerClient = new BlobContainerClient(StorageConnectionString, CocktailImageContainer);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        ms.Position=0;
        await blobClient.UploadAsync(ms);
        
        // ms.Position=0;
        // using Image UploadThumb = Image.ThumbnailStream(ms,128,crop: Enums.Interesting.Attention);
        // //UploadThumb.WriteToFile(imagePath + imageName + "Att" + imageExt);
        // UploadThumb.WriteToStream(ms_t,"jpg");       
        

        // await blobClient.UploadAsync(ms_t,OverWrite);
        

    }

    public async Task UploadToStorage()
    {
        string StorageConnectionString = _configuration["AZURE_STORAGE_CONNECTION_STRING"];
        string CocktailImageContainer = _configuration["CocktailImageContainer"];

        // Create a BlobServiceClient object which will be used to create a container client
        //BlobServiceClient blobServiceClient = new BlobServiceClient(StorageConnectionString);
        // Create the container and return a container client object
        BlobContainerClient containerClient = new BlobContainerClient(StorageConnectionString, CocktailImageContainer);


        string localPath = "./images/";
        string fileName = "cocktail" + Guid.NewGuid().ToString() + ".jpg";
        string localFilePath = Path.Combine(localPath, fileName);

        fileName = "./images/empress.jpg";
        if (System.IO.File.Exists(fileName))
        {
            // To Do
        };

        // Get a reference to a blob
        //fileName = "belvedere.jpg";
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(fileName, true);

    }
}

