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
            using var message = new MailMessage();
            message.From = new MailAddress(_emailSettings.Email);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml=false;
        

        using var client =new SmtpClient(_emailSettings.Host, _emailSettings.Port);

        client.Credentials = new NetworkCredential(
            _emailSettings.Email,
            _emailSettings.AppPassword
        );

        client.EnableSsl=true;

        await client.SendMailAsync(message);
        }
    }
}