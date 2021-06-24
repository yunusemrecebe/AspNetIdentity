using System.Net.Mail;
using System.Net;

namespace AspNetIdentity.Helpers
{
    public static class PasswordReset
    {
        public static void PasswordResetSendEmail(string link)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage mail = new MailMessage();

            smtpClient.Port = 587;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("insanevladi.smtp@gmail.com", "yunusemrecebe123");

            mail.Subject = "E-Posta Konusu"; mail.IsBodyHtml = true; mail.Body = "E-Posta İçeriği";
            mail.Body = "<h2>Parolanızı sıfırlamak için lütfen aşağıdaki linke tıklayınız</h2><hr/>";
            mail.Body += $"<a href='{link}'> Parola Sıfırlama Linki </a>";
            mail.IsBodyHtml = true;
            mail.From = new MailAddress("insanevladi.smtp@gmail.com", "Identity Server::Parola Sıfırlama");
            mail.To.Add("yunusemrecebe@gmail.com");

            smtpClient.Send(mail);
        }
    }
}
