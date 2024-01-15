using CourierHub.IntegrationTest.ApiModels;
using System.Net.Http.Json;
using System.Text.Json;

namespace CourierHub.IntegrationTest.StepDefinitions;

[Binding]
public sealed class MakeInquireStepDefinitions {
    private readonly HttpClient _httpClient = new();
    private ApiInquire? _inquire;
    private HttpResponseMessage? _response;

    [Given("the base address for MakeInquire is (.*)")]
    public void GivenTheBaseAddressForMakeInquireIs(string address) {
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.BaseAddress = new Uri(address);
    }

    [Given("a user with inquire (.*)")]
    public void GivenAUserWithInquire(string inqJson) {
        _inquire = JsonSerializer.Deserialize<ApiInquire>(inqJson) ??
            throw new Exception("Provided inquire was in wrong format.");
    }

    [When("he makes a request to (.*)")]
    public async Task WhenHeMakesARequestTo(string endpoint) {
        _response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}{endpoint}", _inquire);
    }

    [Then("he receives an offer (.*)")]
    public async Task ThenHeReceivesAnOffer(string offJson) {
        if (_response == null) {
            throw new Exception("The response of the PostInquireGetOffer request was null.");
        }
        var offers = await _response.Content.ReadFromJsonAsync<ApiOffer[]>();
        if (offers == null || offers.Length == 0) {
            throw new Exception("The response of the PostInquireGetOffer contained no offers.");
        }
        var offer = JsonSerializer.Deserialize<ApiOffer>(offJson) ??
            throw new Exception("Provided offer was in wrong format.");
        var aimed = offers.FirstOrDefault(o => o.Price == offer.Price && o.Code == offer.Code) ??
            throw new Exception("User did not receive expected offer.");
    }
}