﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
//using System.Net.Mail;
using System.Net;
//using System.Net.Mail;
using System.Text;
using Wasla_Auth_App.Models;

namespace Wasla_Auth_App.Services
{
    public class MailService : IMailService
    {
        MailSettings Mail_Settings = null;
        public MailService(IOptions<MailSettings> options)
        {
            Mail_Settings = options.Value;
        }
        public bool SendMail(MailData Mail_Data)
        {
        
            try
            {
                string htmlBody = File.ReadAllText(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "temp.html"));
               // var htmlBody = System.IO.File.ReadAllLines(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "temp.html"));
               // string htmlBody = "<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8' /><meta name='viewport' content='width=device-width, initial-scale=1.0' /><title>Document</title></head><body> <style>body {height:800px;min-height: 100vh;overflow: hidden;margin: 0;}.bg {background: #542d72;background-color: #542d72;width: 100%;height: 500px;padding: 0 20px;overflow: hidden;margin: 0;}.bg-content {background: white;background-color: white;padding: 50px;border-radius: 6px;width: 1000px;min-width: 1000px;height: 400px;margin: 100px auto;position: relative;}h2 {color: #00bc82;}p {font-size: 14px;color: #666;}p > a {color: #542d72; background-color: transparent !important;text-decoration: underline;}.greenbtn {background-color: #542d72;height: 40px;width: 150px;margin: 30px 0;text-align: center;line-height: 2.5;display: block;color: white;text-decoration: none;border-radius: 6px;}.footer {position: absolute;left: 0;bottom: 0;width: 100%;background-color: white;color: #222;height: 60px;border-radius: 0 0 6px 6px;}.left {float: left;clear: both;}.left > ul {list-style: none;}.left > ul li { display: inline-block;margin: 0 20px;}.left > ul li a {color: #00bc82;text-decoration: underline;}.right {float: right;clear: both;}</style><div class='bg'><div class='bg-content'><p>Dear "+ Mail_Data.EmailToName + "</p><h2>Thank you for registering with Wasla!</h2><p>By creating an account, I agree to <a href=''>Terms of Use</a> and<a href=''>Privacy Policy</a> and to receive emails. </p><p>For further information, please <a>contact support.</a></p> <p>Best regards,</p><footer class='footer'><div><img src=\"images/QRCode.png\"/></div></footer></div></div></body></html>";
                //MimeMessage - a class from Mimekit
                MimeMessage email_Message = new MimeMessage();
                MailboxAddress email_From = new MailboxAddress(Mail_Settings.Name, Mail_Settings.EmailId);
                email_Message.From.Add(email_From);
                MailboxAddress email_To = new MailboxAddress(Mail_Data.EmailToName, Mail_Data.EmailToId);
                email_Message.To.Add(email_To);
                email_Message.Subject = Mail_Data.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = htmlBody;
                emailBodyBuilder.TextBody = htmlBody;
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

        public bool SendOTPMail(MailData Mail_Data)
        {

            try
            {
               
                MimeMessage email_Message = new MimeMessage();
                MailboxAddress email_From = new MailboxAddress(Mail_Settings.Name, Mail_Settings.EmailId);
                email_Message.From.Add(email_From);
                MailboxAddress email_To = new MailboxAddress(Mail_Data.EmailToName, Mail_Data.EmailToId);
                email_Message.To.Add(email_To);
                email_Message.Subject = Mail_Data.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = Mail_Data.EmailBody;
                emailBodyBuilder.TextBody = Mail_Data.EmailBody;
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

        //public bool SendMail(MailData Mail_Data)
        //{
        //    string htmlBody = "<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8' /><meta name='viewport' content='width=device-width, initial-scale=1.0' /><title>Document</title></head><body> <style>body {min-height: 100vh;overflow: hidden;margin: 0;}.bg {background-color: #542d72;width: 100%;height: 100vh;padding: 0 20px;overflow: hidden;margin: 0;}.bg-content {background-color: white;padding: 50px;border-radius: 6px;width: 1000px;min-width: 1000px;height: 400px;margin: 100px auto;position: relative;}h2 {color: #00bc82;}p {font-size: 14px;color: #666;}p > a {color: #542d72; background-color: transparent !important;text-decoration: underline;}.greenbtn {background-color: #542d72;height: 40px;width: 150px;margin: 30px 0;text-align: center;line-height: 2.5;display: block;color: white;text-decoration: none;border-radius: 6px;}.footer {position: absolute;left: 0;bottom: 0;width: 100%;background-color: white;color: #222;height: 60px;border-radius: 0 0 6px 6px;}.left {float: left;clear: both;}.left > ul {list-style: none;}.left > ul li { display: inline-block;margin: 0 20px;}.left > ul li a {color: #00bc82;text-decoration: underline;}.right {float: right;clear: both;}</style><div class='bg'><div class='bg-content'><p>Dear User</p><h2>Thank you for registering with Wasla!</h2><p>Activate your account by clicking the button below.</p><p>By creating an account, I agree to <a href=''>Terms of Use</a> and<a href=''>Privacy Policy</a> and to receive emails. </p><a class='greenbtn' href='#'>Activate Account</a> <p>For further information, please <a>contact support.</a></p> <p>Best regards,</p><footer class='footer'><div class='left'><ul><li><a>Home Page</a></li><li><a>FAQs</a></li></ul></div><div class='right'> </div></footer></div></div></body></html>";
        //    try
        //    {
        //        // Set up SMTP client
        //        System.Net.Mail.SmtpClient client = new SmtpClient(Mail_Settings.Host, Mail_Settings.Port);
        //        client.EnableSsl = true;
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = new NetworkCredential(Mail_Settings.EmailId, Mail_Settings.Password);

        //        // Create email message
        //        MailMessage mailMessage = new MailMessage();
        //        mailMessage.From = new MailAddress(Mail_Settings.EmailId);
        //        mailMessage.To.Add(Mail_Data.EmailToId);
        //        mailMessage.Subject = Mail_Data.EmailSubject;
        //        mailMessage.IsBodyHtml = true;
        //        mailMessage.Body = htmlBody;
        //        //StringBuilder mailBody = new StringBuilder();
        //        //mailBody.Append(htmlBody);
        //       // mailBody.AppendFormat(htmlBody);
        //        //mailBody.AppendFormat("<br />");
        //        //mailBody.AppendFormat("<p>Thank you For Registering account</p>");
        //       // mailMessage.Body = mailBody.ToString();

        //        // Send email
        //        client.Send(mailMessage);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Exception Details
        //        return false;
        //    }
        //}


    }
}
