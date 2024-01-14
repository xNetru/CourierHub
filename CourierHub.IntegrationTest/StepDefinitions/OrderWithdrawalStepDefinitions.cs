using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace CourierHub.IntegrationTest.StepDefinitions;

[Binding]
public sealed class OrderWithdrawalStepDefinitions {
    private readonly HttpClient _httpClient = new();
    private HttpResponseMessage? _response;

    [Given("the base address for OrderWithdrawal is (.*)")]
    public void GivenTheBaseAddressForOrderWithdrawalIs(string address) {
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.BaseAddress = new Uri(address);
    }

    [Given("a user having an order with code (.*)")]
    public async Task GivenAUserHavingAnOrderWithCode(string code) {
        //var response = await _httpClient.PatchAsync($"{_httpClient.BaseAddress}Order/{code}/status", new JsonContent(StatusType.NotConfirmed));
        throw new PendingStepException();
    }

    [When("he requests withdrawal at (.*)")]
    public void HeRequestsWithrawalAt(string endpoint) {
        throw new PendingStepException();
        //var response = await Http.PatchAsJsonAsync($"{Http.BaseAddress}Api/{service}/cancel/{orderCode}", service); // fake content for PATCH
        //
    }

    [Then("he receives an anwser (.*)")]
    public void HeReceivesAnAnwser(int status) {
        throw new PendingStepException();
    }
}