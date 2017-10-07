
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace JH_Banking.Models.Helpers
{
    public class EmailHelper
    {
        public static bool SendCredentials(string username, string password,string receiver)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    //smtpClient.EnableSsl = true;
                    //smtpClient.Host = "smtp.gmail.com";
                    //smtpClient.Port = 587;
                    //smtpClient.UseDefaultCredentials = true;
                    //smtpClient.Credentials = new NetworkCredential("jhbankingtest@gmail.com", "jhbankingtest1");
                    string sender = ConfigurationManager.AppSettings["appEmail"].ToString();
                    MailMessage msg = new MailMessage
                    {
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(sender),
                        Subject = "JH_Banking credentials",
                        Body = "Username = "+username+"<br />Password = "+ password,
                        Priority = MailPriority.Normal
                    };
                    msg.To.Add(receiver);
                    smtpClient.Send(msg);
                    return true;
                }
            }
            catch (System.Exception e)
            {
                return false;
            }
        }
    }
}