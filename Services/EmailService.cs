using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace CareerPilot_AI.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(new MailboxAddress("CareerPilot AI", _configuration["EmailSettings:Email"]));

            email.To.Add(new MailboxAddress("", toEmail));

            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            smtp.Connect(
                 _configuration["EmailSettings:Host"],
                 int.Parse(_configuration["EmailSettings:Port"]),
                 SecureSocketOptions.StartTls);

            smtp.Authenticate(
                _configuration["EmailSettings:Email"],
                _configuration["EmailSettings:Password"]);

            smtp.Send(email);

            smtp.Disconnect(true);


        }




    }
         

    }

