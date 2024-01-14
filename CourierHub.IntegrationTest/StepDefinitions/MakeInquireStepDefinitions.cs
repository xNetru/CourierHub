namespace CourierHub.IntegrationTest.StepDefinitions;
[Binding]
public sealed class MakeInquireStepDefinitions {

    public MakeInquireStepDefinitions() {

    }

    [Given("a user with inquire (.*)")]
    public void GivenAUserWithInquire(string inquire) {
        throw new PendingStepException();
    }

    [When("he makes a request to (.*)")]
    public void WhenHeMakesARequestTo(string endpoint) {
        throw new PendingStepException();
    }

    [Then("he receives an offer (.*)")]
    public void ThenHeReceivesAnOffer(string offer) {
        throw new PendingStepException();
    }
}