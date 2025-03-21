using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SonicStore.Common.Utils
{
    public class EmailService
    {
        private readonly string _fromEmail = "thuyhnhe176007@fpt.edu.vn";
        private readonly string _fromPassword = "eplw raxe tcpr rrlk"; // Nên sử dụng cấu hình trong appsettings.json
        private readonly string _smtpHost = "smtp.gmail.com";
        private readonly int _smtpPort = 587;

        public async Task<bool> SendOTPEmail(string toEmail, int otp)
        {
            try
            {
                var fromAddress = new MailAddress(_fromEmail);
                var toAddress = new MailAddress(toEmail);
                const string subject = "OTP Code";
                string body = $@"
                    <html>
                    <body>
                        <h2>Your OTP Code</h2>
                        <h1>{otp}</h1>
                        <p>Please use this code to complete your transaction.</p>
                    </body>
                    </html>
                ";

                using (var message = new MailMessage(fromAddress, toAddress))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        smtp.Host = _smtpHost;
                        smtp.Port = _smtpPort;
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(fromAddress.Address, _fromPassword);
                        smtp.Timeout = 200000;

                        await smtp.SendMailAsync(message);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendPasswordResetLink(string toEmail, string resetLink)
        {
            try
            {
                var fromAddress = new MailAddress(_fromEmail);
                var toAddress = new MailAddress(toEmail);
                const string subject = "Password Reset";
                string body = $@"
                    <html>
                    <body>
                        <h2>Password Reset</h2>
                        <p>Please click the link below to reset your password:</p>
                        <p><a href='{resetLink}'>Reset Password</a></p>
                    </body>
                    </html>
                ";

                using (var message = new MailMessage(fromAddress, toAddress))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        smtp.Host = _smtpHost;
                        smtp.Port = _smtpPort;
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(fromAddress.Address, _fromPassword);
                        smtp.Timeout = 200000;

                        await smtp.SendMailAsync(message);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
