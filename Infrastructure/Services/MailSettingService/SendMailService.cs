using MailKit.Security;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit;
using Domain.Interfaces;

namespace Infrastructure.Services.MailSettinService
{
    public class SendMailService : ISendMailService
    {
        private readonly MailSetting mailSetting;

        public SendMailService(IOptions<MailSetting> mailSetting)
        {
            this.mailSetting = mailSetting.Value;
        }
        public async Task<bool> SendMail(string MailTo, string Subject, string Body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(mailSetting.Email),
                Subject = Subject
            };

            email.To.Add(MailboxAddress.Parse(MailTo));

            var builder = new BodyBuilder();

            builder.HtmlBody = Body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(mailSetting.DisplayName, mailSetting.Email));

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(mailSetting.Host, mailSetting.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(mailSetting.Email, mailSetting.Password);
            var result = await smtp.SendAsync(email);

            smtp.Disconnect(true);
            var status = result.Split(" ")[0];
            if (status == "2.0.0")
                return true;
            else
                return false;
        }
    }
}
