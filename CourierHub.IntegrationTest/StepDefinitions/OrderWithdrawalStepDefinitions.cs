namespace CourierHub.IntegrationTest.StepDefinitions; 
[Binding]
public sealed class OrderWithdrawalStepDefinitions {
    [Given("a user having an order with code (.*)")]
    public void GivenAUserHavingAnOrderWithCode(string code) {
        throw new PendingStepException();
    }

    [When("he requests withrawal at (.*)")]
    public void HeRequestsWithrawalAt() {
        throw new PendingStepException();
    }

    [Then("he receives an anwser (.*)")]
    public void HeReceivesAnAnwser(int status) {
        throw new PendingStepException();
    }
}