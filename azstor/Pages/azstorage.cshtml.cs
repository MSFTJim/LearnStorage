using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;

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
        // do something with FormData

        // if (string.IsNullOrEmpty(FormData))
        {
            // To Do
        };

        var ms = new MemoryStream();
        if (Upload.Length > 0)
        {
            await Upload.CopyToAsync(ms);

        }

        // if (Upload.Length > 0)        
        //     using (var ms = new MemoryStream())        
        //         await Upload.CopyToAsync(ms);


        string StorageConnectionString = _configuration["AZURE_STORAGE_CONNECTION_STRING"];
        string CocktailImageContainer = _configuration["CocktailImageContainer"];
        string fileName = "cocktail" + Guid.NewGuid().ToString() + ".jpg";

        BlobContainerClient containerClient = new BlobContainerClient(StorageConnectionString, CocktailImageContainer);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        ms.Position=0;
        await blobClient.UploadAsync(ms);
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

