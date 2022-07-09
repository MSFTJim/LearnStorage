using Microsoft.AspNetCore.Hosting;

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


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/upload", 
    async Task<IResult> (HttpRequest request) =>
        {
            if (!request.HasFormContentType)
                return Results.BadRequest();

            var form = await request.ReadFormAsync();
            var formFile = form.Files["file"];

            if (formFile is null || formFile.Length == 0)
                return Results.BadRequest();

            await using var stream = formFile.OpenReadStream();

            var reader = new StreamReader(stream);
            var text = await reader.ReadToEndAsync();

            return Results.Ok(text);
        });

var RouteHandler = new ImageHandler();
app.MapPost("/ImageStor", RouteHandler.WriteImagetoStorage);

string webRootPath = app.Environment.WebRootPath;

app.Run();



class ImageHandler
{
    public async Task<IResult> WriteImagetoStorage(string webRootPath, HttpRequest request)
    {
         if (!request.HasFormContentType)
                return Results.BadRequest();

            var form = await request.ReadFormAsync();
            var formFile = form.Files["file"];

            if (formFile is null || formFile.Length == 0)
                return Results.BadRequest();

            await using var stream = formFile.OpenReadStream();

            var reader = new StreamReader(stream);
            var text = await reader.ReadToEndAsync();
            //var junk = AppCon

            string uploadsFolder = Path.Combine(webRootPath, "images");
            
            
            string filePath = Path.Combine(uploadsFolder, formFile.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    //ctImage.CopyTo(fileStream);
                }

            return Results.Ok(text);
        //return "Hello from the WriteImagetoStorage Instance method handler!";
    }
}

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}