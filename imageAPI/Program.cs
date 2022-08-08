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
    public async Task<string> WriteImagetoStorage(IConfiguration config, HttpRequest request)
    {
        int dog = 0;

        dog = TestMeth();

        if (!request.HasFormContentType)
            return "No Form content Type";

        var form = await request.ReadFormAsync();
        var formFile = form.Files["file"];

        string uploadsFolder = Path.Combine(config["Dog"], "images");

        if (formFile is null || formFile.Length == 0)
            return "File size invalid";

        await using var stream = formFile.OpenReadStream();

        // var reader = new StreamReader(stream);
        // var text = await reader.ReadToEndAsync();

        string filePath = Path.Combine(uploadsFolder, formFile.FileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);

        }

        return "API Process was good";

    }

    private int TestMeth()
    {
        return 1;
    }
} //End Class ImageHandler

