Feature: OrderWithdrawal

Scenario: User wants to withdraw an order
Given a user having an order with code code
When he requests withrawal at Api/serviceName/cancel/code
Then he receives an anwser 200