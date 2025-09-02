namespace BGT_Web_Account.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string ToEmail, string subject, string body);
    }
}
