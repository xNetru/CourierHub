using System.Text;
using System.Text.Json;

namespace CourierHub.IntegrationTest.StepDefinitions;

[Binding]
public sealed class OrderWithdrawalStepDefinitions {
    private readonly HttpClient _httpClient = new();
    private HttpResponseMessage? _response;
    private string? _code;

    [Given("the base address for OrderWithdrawal is (.*)")]
    public void GivenTheBaseAddressForOrderWithdrawalIs(string address) {
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.BaseAddress = new Uri(address);
    }

    [Given("a user having an order with code (.*)")]
    public async Task GivenAUserHavingAnOrderWithCode(string code) {
        string status = JsonSerializer.Serialize(StatusType.NotConfirmed);
        var response = await _httpClient.PatchAsync(
            $"{_httpClient.BaseAddress}Order/{code}/status",
            new StringContent(status, Encoding.UTF8, "application/json")
        );
        if (code != "0") {
            response.EnsureSuccessStatusCode();
        }
        _code = code;
    }

    [When("he requests withdrawal at (.*)")]
    public async Task HeRequestsWithrawalAt(string endpoint) {
        _response = await _httpClient.PatchAsync($"{_httpClient.BaseAddress}{endpoint}{_code}", new StringContent(""));
    }

    [Then("he receives an anwser (.*)")]
    public void HeReceivesAnAnwser(int status) {
        if (_response == null) {
            throw new Exception("The response of the PutOrderWithrawal request was null.");
        }
        if ((int)_response.StatusCode != status) {
            throw new Exception("The status code of the response of the PutOrderWithrawal request was different then expected.");
        }
    }
}