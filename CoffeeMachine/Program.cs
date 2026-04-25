var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

int brewCounter = 0;

app.MapGet("/brew-coffee", () =>
{
    var now = DateTimeOffset.Now;

    if (now.Month == 4 && now.Day == 1)
    {
        return Results.StatusCode(418);
    }

    brewCounter++;

    if (brewCounter % 5 == 0)
    {
        return Results.StatusCode(503);
    }

    return Results.Ok(new
    {
        message = "Your piping hot coffee is ready",
        prepared = now.ToString("yyyy-MM-ddTHH:mm:sszzz")
    });
});

app.Run();

public partial class Program { }