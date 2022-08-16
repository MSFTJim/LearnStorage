using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

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
app.MapPost("/ImageStor", RouteHandler.WriteImagetoStorage);

app.MapPost("/upload", Bobo);

string webRootPath = app.Environment.WebRootPath;


app.Run();

IResult Bobo()
{
    // method code goes here
    int dog = 0;
    dog = Hoho();
    return Results.Ok("Hello from Bobo");

}

int Hoho()
{
    return 1;
}

public class ImageHandler
{

    private string returnMsg = "Method Start";
    private string[] permittedExtensions = new string[] { ".gif", ".png", ".jpg", ".jpeg" };
    public async Task<string> WriteImagetoStorage(IConfiguration config, HttpRequest request)
    {
        if (!request.HasFormContentType)
            return "No Form content Type";


        var form = await request.ReadFormAsync();
        var formFile = form.Files["file"];
        string uploadsFolder = Path.Combine(config["Dog"], "images");
        var cat = config.GetValue<long>("FileSizeLimit");

        // if (formFile is null)
        //     return "File is null";
        // if (formFile.Length == 0)
        //     return "File too small";
        // if (formFile.Length < config.GetValue<long>("FileSizeLimit"))
        //     return "File too big";
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

            if (IsValidFileExtensionAndSignature(formFile.FileName, ms, permittedExtensions))
            {
                // call method to write to Azure storage
            }
            else
            {
                // return returnMsg;
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


    private bool IsValidFileExtensionAndSignature(string fileName, Stream data, string[] permittedExtensions)
    {
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



        // using (var reader = new BinaryReader(data))
        // {
        //     var signatures = _fileSignature[ext];
        //     var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

        //     bool fileSigCorrect = signatures.Any(signature =>
        //         headerBytes.Take(signature.Length).SequenceEqual(signature));

        //     returnMsg = fileSigCorrect ? "file check good" : "file signiture invalid";

        //     return fileSigCorrect;

        // }

        var reader = new BinaryReader(data);
        // return true;
        try
        {
            var signatures = _fileSignature[ext];
            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

            bool fileSigCorrect = signatures.Any(signature =>
                headerBytes.Take(signature.Length).SequenceEqual(signature));

            returnMsg = fileSigCorrect ? "file check good" : "file signiture invalid";
            return fileSigCorrect;


        }
        finally
        {
            reader.Dispose();

        }

    }


} //End Class ImageHandler

