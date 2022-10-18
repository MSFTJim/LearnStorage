using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using NetVips;
using System.Net.Http.Headers;
using System.Text.Json;

namespace azstor.Pages;


public class MinimalAPIModel : PageModel
{
    private HttpClient APIclient = new HttpClient();
    private readonly IConfiguration _configuration;
    private readonly long _fileSizeLimit;
    //private string returnMsg = "All Good";
    public string? myAPIMessage { get; set; }
    private string[] permittedExtensions = new string[] { ".gif", ".png", ".jpg", ".jpeg" };
    [BindProperty]
    public string? APIRoute { get; set; }
    public string apiUrl { get; set; } = "http://127.0.0.1:5136/";

    public MinimalAPIModel(IConfiguration configuration)
    {
        _configuration = configuration;
        _fileSizeLimit = _configuration.GetValue<long>("FileSizeLimit");

    }
    

    [BindProperty]
    public IFormFile Upload { get; set; } = null!;
    public IFormFile? Upload2 { get; set; }


    public void OnGet()
    {


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
            // return Redirect("/Error?errorFromCaller=" + returnMsg);
        }

        // START of new process to just pass file to API

        apiUrl = apiUrl + APIRoute;
        var myHttpRequest = new HttpRequestMessage(HttpMethod.Post, apiUrl);

        var myMultipartFormData = new MultipartFormDataContent();
        ms.Position = 0;
        var myStreamContent = new StreamContent(ms);
        
        myStreamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(Upload.ContentType);  //Upload.ContentType = "image/jpg"

        myMultipartFormData.Add(myStreamContent, "file", Upload.FileName);

        var response1 = await APIclient.PostAsync(apiUrl, myMultipartFormData);
        
        if (response1.StatusCode == System.Net.HttpStatusCode.OK)                                
            myAPIMessage = await response1.Content.ReadAsStringAsync();  
        else
            myAPIMessage = "HTTP Status Code Not OK!";

        
         return Page();


        // END of new process to just pass file to API
    }
    // public async Task<IActionResult> OldOnPostAsync()
    // {

    //         var ms = new MemoryStream();
    //     var ms2 = new MemoryStream();
    //     if (IsValidFileExtensionAndSignature(Upload.FileName, ms, permittedExtensions))
    //     {
    //         await Upload.CopyToAsync(ms2);
    //         // ms.Dispose();
    //         // await Upload.CopyToAsync(ms);
    //         // var apiUrl = "http://localhost:5136/ImageStor";
    //         var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

    //         var formData = new MultipartFormDataContent();
    //         ms2.Position = 0;
    //         var streamContent = new StreamContent(ms2);

    //         streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(Upload.ContentType);  //Upload.ContentType = "image/jpg"

    //         formData.Add(streamContent);

    //         // var response1 = await APIclient.PostAsync(apiUrl, formData);

    //         // if (response1.StatusCode == System.Net.HttpStatusCode.OK)
    //         // {

    //         //     var content = await response1.Content.ReadAsStreamAsync();
    //         // }

    //         await WritetoAzureStorage(ms, Upload.FileName);
    //         return Redirect("/Index");

    //     }
    //     else
    //     {
    //         return Redirect("/Error?errorFromCaller=" + returnMsg);

    //     }


    // }  // End old Post

    // private async Task WritetoAzureStorage(MemoryStream _ms, string filename)
    // {
    //     string StorageConnectionString = _configuration["AZURE_STORAGE_CONNECTION_STRING"];
    //     string CocktailImageContainer = _configuration["CocktailImageContainer"];
    //     Boolean OverWrite = true;
    //     var ms_t = new MemoryStream();
    //     string fileThumb = "_t";

    //     string trustedExtension = Path.GetExtension(filename).ToLowerInvariant();
    //     string trustedFilenameOnly = Path.GetFileNameWithoutExtension(filename);
    //     int allowedFileNameLength = trustedFilenameOnly.Length < 11 ? trustedFilenameOnly.Length : 10;
    //     string shortFileName = trustedFilenameOnly.Substring(0, allowedFileNameLength);
    //     string trustedNewFileName = Guid.NewGuid().ToString() + "-" + shortFileName;

    //     BlobContainerClient containerClient = new BlobContainerClient(StorageConnectionString, CocktailImageContainer);
    //     BlobClient blobClient = containerClient.GetBlobClient(trustedNewFileName + trustedExtension);

    //     _ms.Position = 0;
    //     await blobClient.UploadAsync(_ms, OverWrite);

    //     _ms.Position = 0;
    //     using Image UploadThumb = Image.ThumbnailStream(_ms, width: 128, height: 128, crop: Enums.Interesting.Attention);
    //     blobClient = containerClient.GetBlobClient(trustedNewFileName + fileThumb + trustedExtension);

    //     UploadThumb.WriteToStream(ms_t, trustedExtension);
    //     ms_t.Position = 0;
    //     await blobClient.UploadAsync(ms_t, OverWrite);
    // }

    // private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
    //     {
    //         { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
    //         { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
    //         { ".jpeg", new List<byte[]>
    //             {
    //                 new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
    //                 new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
    //                 new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
    //             }
    //         },
    //         { ".jpg", new List<byte[]>
    //             {
    //                 new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
    //                 new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
    //                 new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
    //             }
    //         }
    //     };


    // private bool IsValidFileExtensionAndSignature(string fileName, Stream data, string[] permittedExtensions)
    // {
    //     if (data == null || data.Length == 0)
    //     {
    //         returnMsg = "file empty";
    //         return false;
    //     }

    //     var filenameonly = Path.GetFileNameWithoutExtension(fileName);
    //     if (string.IsNullOrEmpty(filenameonly))
    //     {
    //         returnMsg = "file name not valid";
    //         return false;
    //     }

    //     var ext = Path.GetExtension(fileName).ToLowerInvariant();
    //     if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
    //     {
    //         returnMsg = "file extension not valid";
    //         return false;
    //     }

    //     data.Position = 0;



    //     // using (var reader = new BinaryReader(data))
    //     // {
    //     //     var signatures = _fileSignature[ext];
    //     //     var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

    //     //     bool fileSigCorrect = signatures.Any(signature =>
    //     //         headerBytes.Take(signature.Length).SequenceEqual(signature));

    //     //     returnMsg = fileSigCorrect ? "file check good" : "file signiture invalid";

    //     //     return fileSigCorrect;

    //     // }

    //     var reader = new BinaryReader(data);
    //     // return true;
    //     try
    //     {
    //         var signatures = _fileSignature[ext];
    //         var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

    //         bool fileSigCorrect = signatures.Any(signature =>
    //             headerBytes.Take(signature.Length).SequenceEqual(signature));

    //         returnMsg = fileSigCorrect ? "file check good" : "file signiture invalid";
    //         return fileSigCorrect;


    //     }
    //     finally
    //     {
    //         reader.Dispose();

    //     }

    // }
} //End Class

    public  class APIResponse
        {
            ActionResult? result { get; set; }
            string? value { get; set; }
        };

