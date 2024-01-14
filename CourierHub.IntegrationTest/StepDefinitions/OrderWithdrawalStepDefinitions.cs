using static System.Net.WebRequestMethods;

namespace CourierHub.IntegrationTest.StepDefinitions;

[Binding]
public sealed class OrderWithdrawalStepDefinitions {
    private readonly HttpClient _httpClient = new();
    private HttpResponseMessage? _response;

    [Given("the base address for OrderWithdrawal is (.*)")]
    public void GivenTheBaseAddressIs(string address) {
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.BaseAddress = new Uri(address);
    }

    [Given("a user having an order with code (.*)")]
    public void GivenAUserHavingAnOrderWithCode(string code) {
        // before calling cancelation, change status to cancelable (eg. 1)
        throw new PendingStepException();
    }

    [When("he requests withdrawal at (.*)")]
    public void HeRequestsWithrawalAt(string endpoint) {
        throw new PendingStepException();
        //var response = await Http.PatchAsJsonAsync($"{Http.BaseAddress}Api/{service}/cancel/{orderCode}", service); // fake content for PATCH
        //var response = await Http.PatchAsJsonAsync($"{Http.BaseAddress}Order/{Order.Code}/status", StatusType.Confirmed);
    }

    [Then("he receives an anwser (.*)")]
    public void HeReceivesAnAnwser(int status) {
        throw new PendingStepException();
    }
}