Feature: MakeInquire

Scenario: User makes an inquire and receives an offer
Given a user with inquire """{"Email":null,"Depth":100,"Width":100,"Length":50,"Mass":49,"SourceDate":"2025-03-13T00:00:00","DestinationDate":"2025-03-15T00:00:00","Datetime":"2025-02-14T00:00:00","IsCompany":true,"IsWeekend":true,"Priority":1,"Code":"","Destination":{"Street":"Solna","Number":"20","Flat":"1a","PostalCode":"20-192","City":"Warszawa"},"Source":{"Street":"Ursynowska","Number":"2","Flat":"10","PostalCode":"30-182","City":"Toronto"}}"""
When he makes a request to Api/inquire
Then he receives an offer """{"Price":20.696256,"Code":"MjAyNWEybzMxNDEyMDIwMA==","ServiceName":null,"ExpirationDate":"0001-01-01T00:00:00"}"""