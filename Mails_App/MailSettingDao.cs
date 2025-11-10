using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mails_App
{
    public class MailSettingDao
    {
        MailSettings Mail_Settings = null;
        public MailSettingDao(IOptions<MailSettings> options)
        {
            Mail_Settings = options.Value;
        }
        public bool SendMail(MailData Mail_Data)
        {

            try
            {
                //MimeMessage - a class from Mimekit
                MimeMessage email_Message = new MimeMessage();
                MailboxAddress email_From = new MailboxAddress(Mail_Settings.Name, Mail_Settings.EmailId);
                email_Message.From.Add(email_From);
                MailboxAddress email_To = new MailboxAddress(Mail_Data.EmailToName, Mail_Data.EmailToId);
                email_Message.To.Add(email_To);
                email_Message.Subject = Mail_Data.EmailSubject;
                // ✅ Add CC
               List<string>? ccEmails = Mail_Data.ccEmails;
                if (ccEmails != null && ccEmails.Count > 0)
                {
                    foreach (var cc in ccEmails)
                    {
                        email_Message.Cc.Add(new MailboxAddress("", cc));
                    }
                }
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = Mail_Data.EmailBody?.ToString();
                emailBodyBuilder.TextBody = Mail_Data.EmailBody?.ToString();
                if (Mail_Data.withAttatch == true)
                {
                    emailBodyBuilder.Attachments.Add(Mail_Data.FileName, Mail_Data.pdfBytes, new ContentType("application", "pdf"));
                }
                
                email_Message.Body = emailBodyBuilder.ToMessageBody();
                //this is the SmtpClient class from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                SmtpClient MailClient = new SmtpClient();
         
                MailClient.Connect(Mail_Settings.Host, Mail_Settings.Port, Mail_Settings.UseSSL);
                MailClient.Authenticate(Mail_Settings.EmailId, Mail_Settings.Password);
                MailClient.Send(email_Message);
                MailClient.Disconnect(true);
                MailClient.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }

        public bool ContactMail(MailData Mail_Data ,string sender)
        {

            try
            {
                //MimeMessage - a class from Mimekit
                MimeMessage email_Message = new MimeMessage();
                MailboxAddress email_From = new MailboxAddress(sender, sender);
                email_Message.From.Add(email_From);
                MailboxAddress email_To = new MailboxAddress(Mail_Settings.Name, Mail_Settings.EmailId);
                email_Message.To.Add(email_To);
                email_Message.Subject = Mail_Data.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = Mail_Data.EmailBody?.ToString();
                emailBodyBuilder.TextBody = Mail_Data.EmailBody?.ToString();
                email_Message.Body = emailBodyBuilder.ToMessageBody();
                //this is the SmtpClient class from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                SmtpClient MailClient = new SmtpClient();
                MailClient.Connect(Mail_Settings.Host, Mail_Settings.Port, Mail_Settings.UseSSL);
                MailClient.Authenticate(Mail_Settings.EmailId, Mail_Settings.Password);
                MailClient.Send(email_Message);
                MailClient.Disconnect(true);
                MailClient.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }
    }
}
