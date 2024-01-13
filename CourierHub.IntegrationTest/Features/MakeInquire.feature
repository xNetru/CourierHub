Feature: MakeInquire

Scenario: User makes an inquire and receives an offer
Given a user with inquire:
"""
{
  "depth": 0,
  "width": 0,
  "length": 0,
  "mass": 0,
  "sourceAddress": {
    "city": "string",
    "postalCode": "string",
    "street": "string",
    "number": "string",
    "flat": "string"
  },
  "destinationAddress": {
    "city": "string",
    "postalCode": "string",
    "street": "string",
    "number": "string",
    "flat": "string"
  },
  "sourceDate": "2024-01-13T20:04:21.177Z",
  "destinationDate": "2024-01-13T20:04:21.177Z",
  "datetime": "2024-01-13T20:04:21.177Z",
  "isCompany": true,
  "isWeekend": true,
  "priority": 0
}
"""
When he makes a request to Api/inquire/
Then he receives an offer:
"""

"""