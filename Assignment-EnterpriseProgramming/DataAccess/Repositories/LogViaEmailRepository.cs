using DataAccess.context;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DataAccess.Repositories
{
    public class LogViaEmailRepository : ILogRepository
    {
        public void Log(string msg, string ipaddress, string user)
        {
            //Declaring variable with the email sent
            string email = "naydel_1_2@hotmail.com";
            string password = "Casey123!";
            string smtpClient = "smtp.office365.com";
            int port = 587;

            //Creating an SMTPClient service
            SmtpClient smtpclient = new SmtpClient(smtpClient, port);
            smtpclient.EnableSsl = true; //SSL has been enabled with the line
            smtpclient.Credentials = new NetworkCredential(email, password);

            //Instance of message
            MailMessage msgmail = new MailMessage();
            msgmail.From = new MailAddress(email);
            msgmail.To.Add("naydel_1_2@hotmail.com");
            msgmail.Subject = "Log Message";
            msgmail.Body = $"Message: {msg} \n IPAddress: {ipaddress} \n User: {user}";
            smtpclient.Send(msgmail);
        }
        public void Log(Exception exception, string ipaddress, string user)
        {
            //Declaring variable with the email sent
            string email = "naydel_1_2@hotmail.com";
            string password = "Casey123!";
            string smtpClient = "smtp.office365.com";
            int port = 587;

            //Creating an SMTPClient service
            SmtpClient smtpclient = new SmtpClient(smtpClient, port);
            smtpclient.EnableSsl = true; //SSL has been enabled with the line
            smtpclient.Credentials = new NetworkCredential(email, password);

            //Instance of message
            MailMessage msgmail = new MailMessage();
            msgmail.From = new MailAddress(email);
            msgmail.To.Add("naydel_1_2@hotmail.com");
            msgmail.Subject = "Log Message";
            msgmail.Body = $"There has been an error: {exception.Message} \n IPAddress: {ipaddress} \n User: {user}";
            smtpclient.Send(msgmail);

        }
    }
}
