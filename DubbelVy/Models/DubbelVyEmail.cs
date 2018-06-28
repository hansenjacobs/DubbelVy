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

        public static string DisputeSupervisorMessage
        {
            get
            {
                var result = "";
                result += "<p>{0} has submitted a dispute for the audit of {1} on {2}.";
                result += " Please review the dispute for validity and approve or deny the dispute.</p>";
                result += "<p>Upon approval the dispute will be reviewed by the auditing team and decide whether it is valid or not.</p>";
                result += "<h2>Dispute Details</h2>";
                result += "<dl>";
                result += "<dt style='display:inline; font-weight:bold'>Audit Date</dt>";
                result += "<dd style='display:inline'>{3}";
                result += "<dt style='display:inline; font-weight:bold'>Auditee</dt>";
                result += "<dd style='display:inline'>{0}";
                result += "<dt style='display:inline; font-weight:bold'>Audit Template</dt>";
                result += "<dd style='display:inline'>{6}";
                result += "<dt style='display:inline; font-weight:bold'>Work Date</dt>";
                result += "<dd style='display:inline'>{2}";
                result += "<dt style='display:inline; font-weight:bold'>Work Identifier</dt>";
                result += "<dd style='display:inline'>{1}";
                result += "</dl>";
                result += "<h3>Comments</h3>";
                result += "<p>{4}</p>";
                result += "<p><a href='http://localhost:10000/Audits/Details/{5}'>Review Audit</a> | ";
                result += "<a href='http://localhost:10000/Disputes/SupervisorApprove/{5}'>Approve Dispute</a> | ";
                result += "<a href='http://localhost:10000/Disputes/SupervisorDeny/{5}'>Deny Dispute</a></p>";

                return result;
            }
        }
    }
}