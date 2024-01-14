Feature: OrderWithdrawal

Background:
    Given the base address for OrderWithdrawal is https://localhost:7008

Scenario: User wants to withdraw an existing order
Given a user having an order with code MjAyNDExNDE4MjAxNjM=
When he requests withdrawal at Api/CourierHub/cancel/
Then he receives an anwser 201

Scenario: User wants to withdraw non-existing order
Given a user having an order with code 0
When he requests withdrawal at Api/CourierHub/cancel/0
Then he receives an anwser 400