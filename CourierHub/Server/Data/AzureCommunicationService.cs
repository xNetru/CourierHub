using Azure;
using Azure.Communication.Email;
using CourierHub.Shared.Abstractions;
using System.Text;

namespace CourierHub.Server.Data {
    public class AzureCommunicationService : ICloudCommunicationService {
        private readonly EmailClient _emailClient;
        private readonly string _sender;

        public AzureCommunicationService(string connection, string sender) {
            string connectionString = Encoding.UTF8.GetString(Convert.FromBase64String(connection));
            _emailClient = new EmailClient(connectionString);
            _sender = sender;
        }

        public async Task<EmailSendResult?> SendEmailAsync(string recipient, EmailContent content) {
            EmailSendResult? result = null;
            try {
                EmailSendOperation emailSendOperation = await _emailClient.SendAsync(
                    WaitUntil.Completed,
                    _sender,
                    recipient,
                    content.Subject,
                    content.Html);
                result = emailSendOperation.Value;
            } catch (RequestFailedException ex) {
                Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
            }
            return result;
        }
    }
}
