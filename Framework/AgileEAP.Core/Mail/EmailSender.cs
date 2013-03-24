using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Mail;
using AgileEAP.Core;

namespace AgileEAP.Core.Mail
{
    public class EmailSender
    {
        //private static SmtpClient smtpClient;
        //private static object lockObj = new object();

        //static SmtpClient SmtpClient
        //{
        //    get
        //    {
        //        if (smtpClient == null)
        //        {
        //            lock (lockObj)
        //            {
        //                if (smtpClient == null)
        //                {
        //                    string emailServer = Configure.Get<string>("EmailServer", "mail.suntektech.com");
        //                    int emailPort = Configure.Get<int>("EmailPort", 25);
        //                    smtpClient = new SmtpClient(emailServer, emailPort);
        //                    string smtpUser = Configure.Get<string>("EmailUser", "trh");
        //                    string smtpPwd = Configure.Get<string>("EmailPwd", "0ojlmh");
        //                    System.Net.CredentialCache myCache = new System.Net.CredentialCache();
        //                    myCache.Add(emailServer, emailPort, "login", new System.Net.NetworkCredential(smtpUser, smtpPwd));
        //                    smtpClient.Credentials = myCache;
        //                }
        //            }
        //        }

        //        return smtpClient;
        //    }
        //}


        public static void SendMail(MailMessage mail)
        {
            string emailServer = Configure.Get<string>("EmailServer", "mail.suntektech.com");
            int emailPort = Configure.Get<int>("EmailPort", 25);
            string smtpUser = Configure.Get<string>("EmailUser", "trh");
            string smtpPwd = Configure.Get<string>("EmailPwd", "0ojlmh");

            using (SmtpClient smtpClient = new SmtpClient(emailServer, emailPort))
            {
                System.Net.CredentialCache myCache = new System.Net.CredentialCache();
                myCache.Add(emailServer, emailPort, "login", new System.Net.NetworkCredential(smtpUser, smtpPwd));
                smtpClient.Credentials = myCache;

                smtpClient.Send(mail);
            }

            //SmtpClient.Send(mail);
        }

        //public static void SendMailAsync(MailMessage mail)
        //{
        //    string emailServer = Configure.Get<string>("EmailServer", "mail.suntektech.com");
        //    int emailPort = Configure.Get<int>("EmailPort", 25);
        //    SmtpClient smtpClient = new SmtpClient(emailServer, emailPort);
        //    string smtpUser = Configure.Get<string>("EmailUser", "trh");
        //    string smtpPwd = Configure.Get<string>("EmailPwd", "0ojlmh");
        //    System.Net.CredentialCache myCache = new System.Net.CredentialCache();
        //    myCache.Add(emailServer, emailPort, "login", new System.Net.NetworkCredential(smtpUser, smtpPwd));
        //    smtpClient.Credentials = myCache;
        //    smtpClient.SendAsync(mail, null);



        //    //SmtpClient.SendAsync(mail, null);
        //}

        //public static void SendMail(Email mail)
        //{

        //    //SmtpClient.SendAsync(mail, null);
        //}
    }
}
