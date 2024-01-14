﻿namespace CourierHub.IntegrationTest.StepDefinitions;
[Binding]
public sealed class OrderWithdrawalStepDefinitions {
    [Given("a user having an order with code (.*)")]
    public void GivenAUserHavingAnOrderWithCode(string code) {
        // before calling cancelation, change status to cancelable (eg. 1)
        throw new PendingStepException();
    }

    [When("he requests withdrawal at (.*)")]
    public void HeRequestsWithrawalAt(string endpoint) {
        throw new PendingStepException();
    }

    [Then("he receives an anwser (.*)")]
    public void HeReceivesAnAnwser(int status) {
        throw new PendingStepException();
    }
}