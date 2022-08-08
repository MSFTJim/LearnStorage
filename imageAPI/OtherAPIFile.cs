public class OtherAPIFile
{
    // do something here
    public void RegisterEmployeeAPIs(WebApplication app)
    {
        app.MapGet("/employees", () =>
        {
            return Results.Ok();
        });
    }


} // end OtherAPIFile

