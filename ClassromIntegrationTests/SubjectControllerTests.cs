using System.Diagnostics;
using ClassromIntegrationTests.Factories;
using Newtonsoft.Json;

namespace ClassromIntegrationTests;

public class
    SubjectControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;


    public SubjectControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task GetAll_ReturnsOkResult_WithSubjects()
    {
      
        Console.WriteLine("GET kérés indítása: /api/subjects");

      
        var response = await _client.GetAsync("/api/subjects");

        
        response.EnsureSuccessStatusCode();

     
        var responseString = await response.Content.ReadAsStringAsync();

      
        Console.WriteLine("Válasz megérkezett, tartalom hossza: " + responseString.Length);

      
        Assert.Contains("Nyelvtan", responseString);
        Assert.Contains("Irodalom", responseString);
        Assert.Contains("Matematika", responseString);
        Assert.Contains("Angol", responseString);
    }


    /*
    [Fact]
    public async Task GetAll_ReturnsNotFound_WhenNoSubjects()
    {
        var response = await _client.GetAsync("/api/subjects");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("No subjects found", responseString);
    }

    [Fact]
    public async Task GetAll_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        // Hozz létre egy olyan környezetet, amely hibát generál
        var response = await _client.GetAsync("/api/subjects");

        // Ellenőrizzük, hogy 500-as hiba történt
        Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", responseString);
    }*/
}