using System.Net.Mail;

namespace AlVer.Classes
{
    public class EmailHelper
    {
        public bool SendEmail(string email, string body, string subject)
        {
            MailAddress sendMail = new MailAddress("gamecitynoreply@gmail.com");

            MailMessage message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = sendMail;
            message.Subject = subject;
            message.Body = body;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("gamecitynoreply@gmail.com", "jicbgyrprnquksxv");
            client.EnableSsl = true;

            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
