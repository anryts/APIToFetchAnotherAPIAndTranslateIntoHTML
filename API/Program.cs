const string UrlToAPI = "https://randomfox.ca/floof/";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/get_random_fox", async (IHttpClientFactory httpClientFactory) =>
{
    var httpClient = httpClientFactory.CreateClient();

    // Fetch data from the randomfox.ca API
    var response = await httpClient.GetAsync(UrlToAPI);

    if (response.IsSuccessStatusCode)
    {
        // Deserialize the JSON response
        var foxResponse = await response.Content.ReadFromJsonAsync<FoxResponse>();

        // Create an HTML response with the fox image and a header
        var html = $"""
                    <!DOCTYPE html>
                                            <html>
                                            <head>
                                                <title>Random Fox</title>
                                            </head>
                                            <body>
                                                <h1>Cute Random Fox to power up your day !!!</h1>
                                                <img src="{foxResponse.image}" alt="Random Fox">
                                            </body>
                                            </html>
                    """;

        return Results.Content(html, "text/html");
    }

    return Results.StatusCode(404);
});

app.Run();


public class FoxResponse
{
    public string image { get; set; }
    public string link { get; set; }
}