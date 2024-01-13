using Azure.Communication.Email;

namespace CourierHub.Cloud;
public interface ICloudCommunicationService {
    Task<EmailSendResult?> SendEmailAsync(string recipient, EmailContent content, IEnumerable<EmailAttachment> attachments);
}
