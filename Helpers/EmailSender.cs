using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SKTravelsApp.Helpers
{
    public class EmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message, string username)
        {
            try
            {
                var emailMessage = CreateEmailMessage(message, username);

                Send(emailMessage);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        private MimeMessage CreateEmailMessage(Message message, string username)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
                emailMessage.To.AddRange(message.To);
                emailMessage.Subject = message.Subject;

                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = string.Format("<h3 style='color:blue'>{0} </h3>" +
                                      "<h4 style='color:black'>Hi {1}, " +
                                      "<p style='color:black'> Welcome To SK Travels India</p>" +
                                      "</h4><br>" + "contact <strong>sktravelsindiaportal@gmail.com</strong> for your queries related to SK Travels",
                                       message.Content, username.ToUpper())
                };

                return emailMessage;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        public void SendResetPasswordLink(Message message, string username)
        {
            try
            {
                var emailMessage = GenerateResetPasswordLink(message, username);

                Send(emailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MimeMessage GenerateResetPasswordLink(Message message, string username)
        {
            try
            {
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
                emailMessage.To.AddRange(message.To);
                emailMessage.Subject = message.Subject;

                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = string.Format("<h3 style='color:#da3394'>Hi {0}, click the link to reset your password </h3>" +
                                         "<a href={1} style='font-weight:700'>Reset Password Link</a>", myTI.ToTitleCase(username), message.Content)
                };


                return emailMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
