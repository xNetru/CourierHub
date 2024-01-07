using Azure.Communication.Email;

namespace CourierHub.Shared.Abstractions; 
public interface ICloudCommunicationService {
    Task<EmailSendResult?> SendEmailAsync(string recipient, EmailContent content, IEnumerable<EmailAttachment> attachments);
}
