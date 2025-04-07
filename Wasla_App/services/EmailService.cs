//using MailKit.Net.Smtp;
//using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WaslaaSendEmail.Services;

public class EmailService  : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        //try
        //{
        //    var emailSettings = _configuration.GetSection("EmailSettings");

        //var message = new MimeMessage();
        //message.From.Add(new MailboxAddress("Sender", emailSettings["SenderEmail"]));
        //message.To.Add(new MailboxAddress("", toEmail));
        //message.Subject = subject;

        //var bodyBuilder = new BodyBuilder { HtmlBody = body };
        //message.Body = bodyBuilder.ToMessageBody();

        //using (var client = new SmtpClient())
        //{
        //    await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), false);
        //    await client.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);
        //    await client.SendAsync(message);
        //    await client.DisconnectAsync(true);
        //}
        //}
        //catch (SmtpCommandException smtpEx)
        //{
        //    // Catch SMTP-specific exceptions
        //    Console.WriteLine($"SMTP Error: {smtpEx.Message}");
        //    Console.WriteLine($"Status Code: {smtpEx.StatusCode}");
        //}
        //catch (SmtpProtocolException protocolEx)
        //{
        //    // Catch protocol-related exceptions (e.g., incorrect server or port)
        //    Console.WriteLine($"SMTP Protocol Error: {protocolEx.Message}");
        //}
        //catch (Exception ex)
        //{
        //    // Catch all other exceptions and log the error details
        //    Console.WriteLine($"General Error: {ex.Message}");
        //    Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
        //    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        //}
    }
}