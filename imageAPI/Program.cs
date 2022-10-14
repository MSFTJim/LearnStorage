using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NetVips;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

var RouteHandler = new ImageHandler();
app.MapPost("/ImageStor", RouteHandler.ProcessImageforStorage);

string webRootPath = app.Environment.WebRootPath;

app.Run();


public class ImageHandler
{

    private string returnMsg = "Method Start";
    private string[] permittedExtensions = new string[] { ".gif", ".png", ".jpg", ".jpeg" };
    public async Task<string> ProcessImageforStorage(IConfiguration config, HttpRequest request)
    {
        if (!request.HasFormContentType)
            return "No Form content Type";


        var form = await request.ReadFormAsync();
        var formFile = form.Files["file"];
        string uploadsFolder = Path.Combine(config["Dog"], "images");
        
        if (formFile is null || formFile.Length == 0 || formFile.Length > config.GetValue<long>("FileSizeLimit"))
            return "File size invalid";

        await using var stream = formFile.OpenReadStream();

        string filePath = Path.Combine(uploadsFolder, formFile.FileName);

        // using (var fileStream = new FileStream(filePath, FileMode.Create))
        // {
        //     await stream.CopyToAsync(fileStream);
        // }

        using (var ms = new MemoryStream())
        {
            await stream.CopyToAsync(ms);

            // bool CheckMate = IsValidFileExtensionAndSignature(formFile.FileName, ms, permittedExtensions).Result;

            if (IsValidFileExtensionAndSignature(formFile.FileName, ms, permittedExtensions))
            {
                // call method to write to Azure storage
                await stream.CopyToAsync(ms);
                await WritetoAzureStorage(ms, filePath, config);
            }
            else
            {
                return returnMsg;
            }

        }


        return "API Process was good";

    } // End WriteImagetoStorage


    private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
            { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            }
        };


    public bool IsValidFileExtensionAndSignature(string fileName, MemoryStream streamParam, string[] permittedExtensions)
    {        
        returnMsg = "file check start";        

        MemoryStream data = new MemoryStream();
        streamParam.Position =0;
        streamParam.CopyTo(data);

        // this is generally checked by the file upload control itself - but we can double check it here
        if (data == null || data.Length == 0)
        {
            returnMsg = "file empty";
            return false;
        }

        var filenameonly = Path.GetFileNameWithoutExtension(fileName);
        if (string.IsNullOrEmpty(filenameonly))
        {
            returnMsg = "file name not valid";
            return false;
        }

        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        {
            returnMsg = "file extension not valid";
            return false;
        }

        data.Position = 0;

        using (var reader = new BinaryReader(data))
        {
            var signatures = _fileSignature[ext];
            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

            bool fileSigCorrect = signatures.Any(signature =>
                headerBytes.Take(signature.Length).SequenceEqual(signature));

            returnMsg = fileSigCorrect ? "file check good" : "file signiture invalid";

            return fileSigCorrect;

        }       

    } // End IsValidFileExtensionAndSignature


private async Task WritetoAzureStorage(MemoryStream _ms, string filename, IConfiguration diConfiguration)
    {
        string StorageConnectionString = diConfiguration["AZURE_STORAGE_CONNECTION_STRING"];
        string CocktailImageContainer = diConfiguration["CocktailImageContainer"];
        Boolean OverWrite = true;
        var ms_t = new MemoryStream();
        string fileThumb = "_t";

        string trustedExtension = Path.GetExtension(filename).ToLowerInvariant();
        string trustedFilenameOnly = Path.GetFileNameWithoutExtension(filename);
        int allowedFileNameLength = trustedFilenameOnly.Length < 11 ? trustedFilenameOnly.Length : 10;
        string shortFileName = trustedFilenameOnly.Substring(0, allowedFileNameLength);
        string trustedNewFileName = Guid.NewGuid().ToString() + "-" + shortFileName;

        BlobContainerClient containerClient = new BlobContainerClient(StorageConnectionString, CocktailImageContainer);
        BlobClient blobClient = containerClient.GetBlobClient(trustedNewFileName + trustedExtension);

        _ms.Position = 0;
        await blobClient.UploadAsync(_ms, OverWrite);

        _ms.Position = 0;
        using Image UploadThumb = Image.ThumbnailStream(_ms, width: 128, height: 128, crop: Enums.Interesting.Attention);
        blobClient = containerClient.GetBlobClient(trustedNewFileName + fileThumb + trustedExtension);

        UploadThumb.WriteToStream(ms_t, trustedExtension);
        ms_t.Position = 0;
        await blobClient.UploadAsync(ms_t, OverWrite);
        
    }  // end of write to azure storage


} //End Class ImageHandler

