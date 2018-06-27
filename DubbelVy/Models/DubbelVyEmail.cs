using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Dubbelvy
{
    public class DubbelVyEmail
    {
        public static void Send(MailMessage message)
        {
            using (var smtp = new SmtpClient())
            {
                smtp.Send(message);
            }
        }

        public static string AuditCompletedMessage
        {
            get
            {
                var result = "";
                result += "<h2>{0} Audit Completed</h2>";
                result += "<p>- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -<p>";
                result += "<p><b>Auditee: </b>{1}<p/>";
                result += "<p><b>Supervisor: </b>{2}<p/>";
                result += "<p><b>Work Identifier: </b>{3}<p/>";
                result += "<p><b>Work Date/Time: </b>{4}<p/>";
                result += "<p><b>Score: </b>{5}<p/>";
                result += "{6}\r\n\r\n";
                result += "<p><a href='http://localhost:10000/Audits/Details/{7}'>Click here to review the complete audit.</a></p>";
                return result;
            }
        }
    }
}