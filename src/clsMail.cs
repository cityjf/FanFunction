using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace FanFunction
{
    /// <summary>
    /// 邮件发送的类
    /// </summary>
    public class clsMail
    {
        private string sender;
        /// <summary>
        /// 发件箱
        /// </summary>
        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }
        private string senderPwd;
        /// <summary>
        /// 发件箱密码
        /// </summary>
        public string SenderPwd
        {
            get { return senderPwd; }
            set { senderPwd = value; }
        }
        private string smtp;
        /// <summary>
        /// 发件箱smtp服务器地址
        /// </summary>
        public string Smtp
        {
            get { return smtp; }
            set { smtp = value; }
        }
        private string receiver;
        /// <summary>
        /// 收件人，和receiverList不能共存，当只有一个收件人时使用，优先判断Receiver
        /// </summary>
        public string Receiver
        {
            get { return receiver; }
            set { receiver = value; }
        }

        private List<string> receiverList;
        /// <summary>
        /// 收件人邮箱集合，和Receiver不能共存，当有多个收件人时使用，优先判断Receiver
        /// </summary>
        public List<string> ReceiverList
        {
            get { return receiverList; }
            set { receiverList = value; }
        }

        private string subject;
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        private string body;
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Body
        {
            get { return body; }
            set { body = value; }
        }
        //private string from;
        ///// <summary>
        ///// 发件人昵称
        ///// </summary>
        //public string From
        //{
        //    get { return from; }
        //    set { from = value; }
        //}

        /// <summary>
        /// 发送邮件的代码，发邮件之前请先为各个参数赋值
        /// </summary>
        public void SendMail()
        {
            //发件地址
            string strSender = Sender;
            //发件人账号 如果邮箱地址是jwc@shnu.edu.cn 那么发件账号就是jwc
            string strSendId = strSender.Substring(0, strSender.IndexOf('@'));
            //邮件标题根据不同情况显示不同内容
            
            //邮件内容
            string strBody = Body;
            //发件账号密码
            string strSendPwd = SenderPwd;
            //收件人列表,此处为了方便传值，因为经常收件人只有一个
            List<string> arrMailTo;
            if (string.IsNullOrEmpty(Receiver))
            {
                arrMailTo = ReceiverList;
            }
            else
            {
                arrMailTo = new List<string>();
                arrMailTo.Add(Receiver);
            }
            //发件箱的SMTP
            string SMTP = Smtp;

            //发邮件正式开始
            MailAddress from = new MailAddress(strSender, strSender, Encoding.UTF8); //邮件的发件人
            MailMessage mail = new MailMessage();

            //设置邮件的标题
            mail.Subject = Subject;

            //设置邮件的发件人
            //Ps:如果不想显示自己的邮箱地址，这里可以填符合mail格式的任意名称，真正发mail的用户不在这里设定，这个仅仅只做显示用
            mail.From = from;

            //设置邮件的收件人
            foreach (string strMailTo in arrMailTo)
            {
                mail.To.Add(new MailAddress(strMailTo));
            }

            //设置邮件的内容
            mail.Body = strBody;
            //设置邮件的格式
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            //设置邮件的发送级别
            SmtpClient client = new SmtpClient();
            //设置用于 SMTP 事务的主机的名称，填IP地址也可以了
            client.Host = SMTP;
            //设置用于 SMTP 事务的端口，默认的是 25
            //client.Port = 25;
            //这里才是真正的邮箱登陆名和密码，比如我的邮箱地址是 hbgx@hotmail.com， 我的用户名为 hbgx ，我的密码是 xgbh
            client.Credentials = new System.Net.NetworkCredential(strSendId, strSendPwd);
            //都定义完了，正式发送了
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("发送邮件出错了，请尽量使用163邮箱发送邮件！" + ex.Message);
            }
        }
    }
}
