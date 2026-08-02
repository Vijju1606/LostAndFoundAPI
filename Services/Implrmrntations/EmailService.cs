using LostAndFoundAPI.Services.Interfaces;
using LostAndFoundAPI.Common;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;



namespace LostAndFoundAPI.Services.Implementations
{
    public class EmailService : IEmailService
    {

      private readonly EmailSettings _emailSettings;
      public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings=options.Value;
        }



        public async Task SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(_emailSettings.Host)
                || string.IsNullOrWhiteSpace(_emailSettings.Email)
                || string.IsNullOrWhiteSpace(_emailSettings.AppPassword))
            {
                throw new InvalidOperationException("Email delivery is not configured.");
            }

            using var message = new MailMessage();
            message.From = new MailAddress(_emailSettings.Email);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml=false;

            using var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port);

            client.Credentials = new NetworkCredential(
                _emailSettings.Email.Trim(),
                // Gmail displays app passwords in groups of four characters.
                // SMTP requires the actual 16-character value without spaces.
                _emailSettings.AppPassword.Replace(" ", string.Empty).Trim()
            );

            client.EnableSsl = true;
            client.Timeout = 30000;

            await client.SendMailAsync(message);
        }
    }
}
