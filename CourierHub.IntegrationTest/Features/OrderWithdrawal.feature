Feature: OrderWithdrawal

Scenario: User wants to withdraw an order
Given a user having an order with code MjAyNDExNDE4MjAxNjM=
When he requests withdrawal at Api/CourierHub/cancel/MjAyNDExNDE4MjAxNjM=
Then he receives an anwser 201