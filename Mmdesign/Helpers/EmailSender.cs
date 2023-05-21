using log4net;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace Mmdesign.Helpers
{
    public static class EmailSender
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void SendMail(Exception ex)
        {
#if DEBUG
            return;
#endif
            int priorityInt = 0;
            string smtpHost = string.Empty;
            int smtpPort = 0;
            bool enableSSL;
            string displayName = string.Empty;
            string userName = string.Empty;
            string password = string.Empty;
            string[] toRecipientList;
            string[] ccRecipientList;
            string[] bccRecipientList;
            string html = ex.ToString();

            //MailSettingSmtpHost
            smtpHost = ConfigurationManager.AppSettings["MailSettingSmtpHost"];
            if (string.IsNullOrEmpty(smtpHost))
            {
                throw new Exception("Web.config => appSettings.MailSettingSmtpHost");
            }

            //MailSettingSmtpPort
            string portConfig = ConfigurationManager.AppSettings["MailSettingSmtpPort"];
            if (!int.TryParse(portConfig, out smtpPort) || smtpPort <= 0)
            {
                throw new Exception("Web.config => appSettings.MailSettingSmtpPort");
            }

            //MailSettingUserName
            userName = GetMailSettingSecuredConfig("MailSettingUserName");
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("Web.config => appSettings.MailSettingUserName");
            }

            //MailSettingDisplayName
            displayName = ConfigurationManager.AppSettings["MailSettingDisplayName"];
            if (string.IsNullOrEmpty(displayName))
            {
                throw new Exception("Web.config => appSettings.MailSettingDisplayName");
            }

            //MailSettingPassword
            password = GetMailSettingSecuredConfig("MailSettingPassword");
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Web.config => appSettings.MailSettingPassword");
            }

            //MailSettingPriority
            string priorityConfig = ConfigurationManager.AppSettings["MailSettingPriority"];
            if (string.IsNullOrEmpty(priorityConfig)
                || !int.TryParse(priorityConfig, out priorityInt)
                || (priorityInt != 0 && priorityInt != 1 && priorityInt != 2))
            {
                throw new Exception("Web.config => appSettings.MailSettingPriority");
            }

            //MailSettingEnableSSL
            int enableSSLInt = 0;
            string enableSSLConfig = ConfigurationManager.AppSettings["MailSettingEnableSSL"];
            if (string.IsNullOrEmpty(enableSSLConfig)
                || !int.TryParse(enableSSLConfig, out enableSSLInt)
                || (enableSSLInt != 0 && enableSSLInt != 1))
            {
                throw new Exception("Web.config => appSettings.MailSettingEnableSSL");
            }
            enableSSL = enableSSLInt == 1;

            //New MailMessage
            MailMessage message = new MailMessage();

            //From:
            message.From = new MailAddress(userName, displayName, Encoding.UTF8);
            message.Sender = new MailAddress(userName, displayName, Encoding.UTF8);

            //MailSettingToEmailList
            string toRecipients = ConfigurationManager.AppSettings["MailSettingToEmailList"];
            if (string.IsNullOrEmpty(toRecipients))
            {
                throw new Exception("Web.config => appSettings.MailSettingToEmailList");
            }
            else
            {
                toRecipientList = toRecipients.Split(AppConstants.StringDelimiters.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var toEmail in toRecipientList)
                {
                    var mailAddress = new MailAddress(toEmail);
                    message.To.Add(mailAddress);
                }

                //MailSettingToEmailList
                if (toRecipientList.Length <= 0)
                {
                    throw new Exception("Web.config => appSettings.MailSettingToEmailList");
                }
            }

            //MailSettingCcEmailList
            string ccRecipients = ConfigurationManager.AppSettings["MailSettingCcEmailList"];

            //CC:
            if (!string.IsNullOrEmpty(ccRecipients))
            {
                ccRecipientList = ccRecipients.Split(AppConstants.StringDelimiters.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var ccEmail in ccRecipientList)
                {
                    var mailAddress = new MailAddress(ccEmail);
                    message.CC.Add(mailAddress);
                }
            }

            //MailSettingBccEmailList
            string bccRecipients = ConfigurationManager.AppSettings["MailSettingBccEmailList"];

            //CC:
            if (!string.IsNullOrEmpty(bccRecipients))
            {
                bccRecipientList = bccRecipients.Split(AppConstants.StringDelimiters.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var bccEmail in bccRecipientList)
                {
                    var mailAddress = new MailAddress(bccEmail);
                    message.Bcc.Add(mailAddress);
                }
            }

            //Subject
            message.Subject = "[mmdesign.vn] Exception";
            message.SubjectEncoding = Encoding.UTF8;

            //Body:
            message.IsBodyHtml = true;
            message.Body = html;
            message.BodyEncoding = Encoding.UTF8;

            //Set Priority to MailMessage (0: Low, 1: Normal, 2: High)
            if (priorityInt == 2)
            {//High
                message.Priority = MailPriority.High;
            }
            else if (priorityInt == 1)
            {//Normal
                message.Priority = MailPriority.Normal;
            }
            else
            {//Low
                message.Priority = MailPriority.Low;
            }

            //New SmtpClient
            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                try
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(userName, password);
                    client.Timeout = int.MaxValue;
                    client.Send(message);
                }
                catch (Exception exc)
                {
                    log.Error(exc);
                    throw;
                }
            }
        }

        public static string GetMailSettingSecuredConfig(string keyConfig)
        {
            string valueConfig = string.Empty;
            NameValueCollection appConfig = ConfigurationManager.GetSection("MailSettingSecured") as NameValueCollection;
            var cfg = appConfig[keyConfig];
            if (cfg != null)
            {
                valueConfig = cfg;
                var keyConfigLower = keyConfig.ToLowerInvariant();
                if (keyConfigLower.Contains("secret") || keyConfigLower.Contains("password"))
                {
                    var length = cfg.Length;
                    var mid = length / 2;
                    var valueConfigSecured = cfg.Substring(0, mid) + "...";
                    Debug.WriteLine("{0}: {1}", keyConfig, valueConfigSecured);
                }
                else
                {
                    Debug.WriteLine("{0}: {1}", keyConfig, valueConfig);
                }
            }
            return valueConfig;
        }
    }
}