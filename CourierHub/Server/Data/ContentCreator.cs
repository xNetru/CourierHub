using Azure.Communication.Email;
using CourierHub.Shared.ApiModels;
using System.Net.Mime;

namespace CourierHub.Server.Data;
public class ContentCreator {
    private readonly ApiService _service;

    public ContentCreator(ApiService service) {
        _service = service;
    }

    public EmailAttachment CreateMailAttachment(string title, string content) {
        var binaryContent = new BinaryData(content);
        return new EmailAttachment(title, MediaTypeNames.Text.Plain, binaryContent);
    }

    public EmailContent CreateMailContent(ApiMailContent content) {
        int index = content.Link.LastIndexOf('/');
        string code = content.Link.Substring(index + 1);
        string htmlContent =
        $@"
            <!DOCTYPE html>
            <html lang=""pl"">
            <head>
                <meta charset=""UTF-8"">
                <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Status paczki</title>
            </head>
            <body style=""font-family: Arial, sans-serif;"">
            
                <h2>Szanowny/a {content.Client.Name} {content.Client.Surname},</h2>
            
                <p>Mamy nadzieję, że niniejsza wiadomość znajdzie Cię w dobrym zdrowiu. Chcielibyśmy poinformować Cię o aktualnym statusie Twojego zamówienia o kodzie {code}.</p>
            
                <p>Możesz sprawdzić aktualny status swojego zamówienia, klikając na poniższy link:</p>
            
                <p><a href=""{content.Link}"">Sprawdź Status Zamówienia</a></p>
            
                <p>Jeśli masz jakiekolwiek pytania lub obawy, prosimy o kontakt z naszym działem obsługi klienta.</p>
            
                <p>Dziękujemy za wybór naszych usług!</p>
            
                <p>Z poważaniem,<br>
                CourierHub</p>
            
            </body>
            </html>
        ";
        EmailContent email = new EmailContent($"Status zamówienia {code}");
        email.Html = htmlContent;
        return email;
    }

    public string CreateContract(ApiContract contract) {
        string text =
            "----------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n" +
            "                                                        CONTRACT\r\n" +
           $"                                       {contract.Code}\r\n" +
            "\r\n" +
           $"This Agreement is entered into on {contract.DateTime:dd.MM.yyyy}, between:\r\n" +
            "\r\n" +
           $"{(contract.Client.Company ?? contract.Client.Name + " " + contract.Client.Surname)}, hereinafter referred to as the \"Client,\" located at {contract.Client.Address.Street} {contract.Client.Address.Number} {contract.Client.Address.City}\r\n" +
            "" +
            "\r\n" +
            "and\r\n" +
            "\r\n" +
           $"{_service.Name}, hereinafter referred to as the \"Company\".\r\n" +
            "----------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n" +
            "§ 1. Subject of the Agreement\r\n" +
            "----------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n" +
            "1.1. The Client engages the Company to provide parcel delivery services in accordance with the terms and\r\n" +
            "conditions specified in the Company's statute.\r\n" +
            "\r\n" +
            "1.2. The services include pickup, transportation, and delivery of parcels from point A (pickup location) \r\n" +
            "to point B (delivery location).\r\n" +
            "----------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n" +
            "§ 2. Conditions of Service\r\n" +
            "----------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n" +
            "2.1. The Client agrees to provide parcels in accordance with the instructions provided by the Company.\r\n" +
            "\r\n" +
            "2.2. The Company agrees to perform delivery services with due diligence and in compliance with applicable\r\n" +
            "legal regulations.\r\n" +
            "----------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n" +
            "§ 3. Fees\r\n" +
            "----------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n" +
            "3.1. Fees for the provided delivery services are specified in the price list available in Company's statute.\r\n" +
            "\r\n" +
            "3.2. The Client agrees to settle payments for delivery services based on the terms and conditions set by the Company.\r\n" +
            "----------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n" +
            "§ 4. Liability\r\n" +
            "----------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n" +
            "4.1. The Company is responsible for any damages or losses to parcels during delivery unless the damage resulted from" +
            "\r\n" +
            "the Client's fault or was caused by circumstances beyond the Company's control.\r\n" +
            "\r\n" +
            "4.2. The Client is responsible for damages resulting from improper packaging of parcels.\r\n" +
            "----------------------------------------------------------------------------------------------------------------------------------------------------------------------";
        return text;
    }

    public string CreateReceipt(ApiReceipt receipt) {
        string text =
           $"Order code: {receipt.Code}\r\n" +
           $"Date: {receipt.DateTime:dd.MM.yyyy}\r\n" +
            "\r\n" +
            "Order details:\r\n" +
           $"  Depth: {receipt.Inquire.Depth}\r\n" +
           $"  Width: {receipt.Inquire.Width}\r\n" +
           $"  Length: {receipt.Inquire.Length}\r\n" +
           $"  Mass: {receipt.Inquire.Mass}\r\n" +
           $"  Source Date: {receipt.Inquire.SourceDate:dd.MM.yyyy}\r\n" +
           $"  Destination Date: {receipt.Inquire.DestinationDate:dd.MM.yyyy}\r\n" +
           $"  Is Company: {receipt.Inquire.IsCompany}\r\n" +
           $"  Is Weekend: {receipt.Inquire.IsWeekend}\r\n" +
           $"  Priority: {receipt.Inquire.Priority}\r\n" +
            "\r\n" +
            " Destination Address:\r\n" +
           $"   Street: {receipt.Inquire.Destination.Street}\r\n" +
           $"   Number: {receipt.Inquire.Destination.Number}\r\n" +
           $"   Flat: {receipt.Inquire.Destination.Flat}\r\n" +
           $"   City: {receipt.Inquire.Destination.City}\r\n" +
           $"   Postal Code: {receipt.Inquire.Destination.PostalCode}\r\n" +
            "\r\n" +
           $" Source Address:\r\n" +
           $"   Street: {receipt.Inquire.Source.Street}\r\n" +
           $"   Number: {receipt.Inquire.Source.Number}\r\n" +
           $"   Flat: {receipt.Inquire.Destination.Flat}\r\n" +
           $"   City: {receipt.Inquire.Source.City}\r\n" +
           $"   Postal Code: {receipt.Inquire.Source.PostalCode}\r\n" +
            "\r\n" +
            "Client details:\r\n" +
           $"  Email: {receipt.Order.ClientEmail}\r\n" +
           $"  Name: {receipt.Order.ClientName}\r\n" +
           $"  Surname: {receipt.Order.ClientSurname}\r\n" +
           $"  Phone: {receipt.Order.ClientPhone}\r\n" +
           $"  Company: {receipt.Order.ClientCompany}\r\n" +
           $"  Address:\r\n" +
           $"    Street: {receipt.Order.ClientAddress.Street}\r\n" +
           $"    Number: {receipt.Order.ClientAddress.Number}\r\n" +
           $"    Flat: {receipt.Order.ClientAddress.Flat}\r\n" +
           $"    City: {receipt.Order.ClientAddress.City}\r\n" +
           $"    Postal Code: {receipt.Order.ClientAddress.PostalCode}\r\n" +
            "\r\n" +
           $"Price: {receipt.Order.Price}";
        return text;
    }
}
