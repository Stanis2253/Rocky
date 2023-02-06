﻿using Mailjet.Client.TransactionalEmails;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using Org.BouncyCastle.Asn1.Pkcs;
using System.Net.Mail;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Rocky_Utility
{
    public class EmailSender : IEmailSender
    {
        //private readonly ILogger<EmailSender> _emailSender;
        //public string Email { get; set; }
        //public string Subject { get; set; }
        //public string Body { get; set; }
        //public Task SendEmailAsync(string email, string subject, string htmlMessage)
        //{
        //    return Execute(email, subject, htmlMessage);
        //}
        private readonly ILogger<EmailSender> logger;

        private readonly IConfiguration _configuration;


        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            _mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();

            MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey)
            {
                
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "dotnetmastery@protonmail.com"},
        {"Name", "Ben"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "DotNetMastery"
         }
        }
       }
      }, {
       "Subject",
       subject
      }, {
       "HTMLPart",
       body
      }
     }
             });
            await client.PostAsync(request);
        }
    }
}
