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


    public async Task OnGet()
    {
        int dog = 0;
        string StorageConnectionString = _configuration["AZURE_STORAGE_CONNECTION_STRING"];
        string CocktailImageContainer = _configuration["CocktailImageContainer"];

        // Create a BlobServiceClient object which will be used to create a container client
        BlobServiceClient blobServiceClient = new BlobServiceClient(StorageConnectionString);
        // Create the container and return a container client object
        BlobContainerClient containerClient = new BlobContainerClient(StorageConnectionString, CocktailImageContainer);
       // await containerClient.CreateAsync();

        string localPath = "./images/";
        string fileName = "cocktail" + Guid.NewGuid().ToString() + ".jpg";
        string localFilePath = Path.Combine(localPath, fileName);

        fileName = "./images/empress.jpg";
        if (System.IO.File.Exists(fileName))
            dog++;

        // Get a reference to a blob
        fileName = "belvedere.jpg";
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        
     
        await blobClient.UploadAsync(fileName,true);


        dog++;


    }
}

