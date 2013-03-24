using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;

using AgileEAP.Core.Utility;

namespace AgileEAP.Core.UnitTests.Utility
{
    [TestClass]
    public class EmailUtilTest
    {
        [TestMethod]
        public void TestSendMail()
        {
            try
            {
                //MailAddress from = new MailAddress("trh@suntektech.com", "谭任辉");
                //MailAddress to = new MailAddress("xuy@suntektech.com", "徐杨");
                //MailMessage email = new MailMessage(from, to);
                //email.Body = "测试Mail";
                //email.Subject = "测试Mail";

                MailMessage email = new MailMessage("trh@suntektech.com", "xuy@suntektech.com", "test", "test");
                AgileEAP.Core.Mail.EmailSender.SendMail(email);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
        }

        [TestMethod]
        public void TestSendMailAsync()
        {
        }
    }
}
