using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky_Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSettings _emailSettings { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }
        public async Task Execute(string email, string subject, string htmlMessage)
        {
            try
            {
                _emailSettings = _configuration.GetSection("Email").Get<EmailSettings>();

                MimeMessage message = new MimeMessage();

                message.From.Add(new MailboxAddress("Моя компания", _emailSettings.Login)); //отправитель сообщения
                message.To.Add(new MailboxAddress("Client", email)); //адресат сообщения
                message.Subject = subject; //тема сообщения
                message.Body = new BodyBuilder { HtmlBody = htmlMessage }.ToMessageBody();

                using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 465, true); //либо использум порт 465
                    client.Authenticate(_emailSettings.Login, _emailSettings.Password); //логин-пароль от аккаунта
                    client.Send(message);
                    client.Disconnect(true);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
