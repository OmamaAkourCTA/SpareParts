using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SparePartsModule.Infrastructure.ViewModels.Dtos;
using System.Net;
using System.Net.Mail;


namespace SparePartsModule.Core.Helpers
{
    public class EMailService
    {

        private readonly ILogger<EMailService> _logger;
        private readonly IOptionsMonitor<EmailConfiguration> emailConfig;

        private readonly IConfiguration _configuration;
        public EMailService(ILogger<EMailService> logger,
            IOptionsMonitor<EmailConfiguration> emailConfig,
           IConfiguration configuration
            )
        {
            this.emailConfig = emailConfig;
            _logger = logger;
            _configuration = configuration;

        }
        public EMailService()
        {

        }




        public  MimeMessage CreateEmailMessage(Message message)
        {

            var emailMessage = new MimeMessage();
            if (!string.IsNullOrEmpty(emailConfig.CurrentValue.From))
            {
                emailMessage.From.Add(new MailboxAddress(emailConfig.CurrentValue.From, emailConfig.CurrentValue.From));
            } 
            emailMessage.To.AddRange(message.To);
            emailMessage.Cc.AddRange(message.CC);
            emailMessage.Bcc.AddRange(message.BCC);
            emailMessage.Subject = message.Subject;

            BodyBuilder emailBody = new();

            emailBody.HtmlBody = message.Content;//"<p>Employee Name:${}";


            emailMessage.Body = emailBody.ToMessageBody();
            return emailMessage;
        }

        public async Task<bool> SendEmailGmail(string Subject,string Body,string to )
        {



              MailAddress fromAddress = new MailAddress(emailConfig.CurrentValue.UserName ,"Markazia Systems");

            MailMessage mailmsg = new MailMessage
            {
                From = fromAddress
            };

            mailmsg.To.Add(to);

            mailmsg.Body = Body;
            mailmsg.Subject = Subject.Replace('\r', ' ').Replace('\n', ' '); ;
            mailmsg.IsBodyHtml = true;
         

            try
            {
                var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(emailConfig.CurrentValue.UserName, emailConfig.CurrentValue.Password),
                    EnableSsl = true,
                    
                };

                client.Send(mailmsg/*emailConfig.CurrentValue.UserName, "support@lbsts.com", mail.Subject, mail.Body.ToString()*/);


               return  true;
            }
            catch (SmtpException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        public async Task<bool> SendEmail(string Subject, string Body, string to,string? cc=null,string? bcc=null,string? attachment=null)
        {
            MimeMessage mailMessage = new MimeMessage();

           var toEmails= to.Split(",").Where(e=>e!="").Select(e=> new MailboxAddress(e, e)).ToList();
            mailMessage.To.AddRange(toEmails);
            if (!string.IsNullOrEmpty(cc))
            {
                 toEmails = cc.Split(",").Where(e => e != "").Select(e => new MailboxAddress(e, e)).ToList();
                mailMessage.Cc.AddRange(toEmails);
            }
            if (!string.IsNullOrEmpty(bcc))
            {
                toEmails = bcc.Split(",").Where(e => e != "").Select(e => new MailboxAddress(e, e)).ToList();
                mailMessage.Bcc.AddRange(toEmails);
            }
            mailMessage.Subject = Subject;

            BodyBuilder emailBody = new();

            emailBody.HtmlBody = Body;

            //  if(attachment != null) {
            //    var attachmentFile = new MimePart("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            //    {
            //        Content = new MimeContent(File.Open(attachment, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)),
            //        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            //        ContentTransferEncoding = ContentEncoding.Binary,
            //        FileName = Path.GetFileName(attachment)
            //    };
            //    emailBody.Attachments.Add(attachmentFile);
            //}
            if (attachment != null)
            {
                emailBody.Attachments.Add(Directory.GetCurrentDirectory() + "\\" + attachment);
            }
            emailBody.Attachments.Add(Directory.GetCurrentDirectory()+@"\markazia-small.png");

            mailMessage.Body = emailBody.ToMessageBody();
        
           



            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {

                    await client.ConnectAsync(emailConfig.CurrentValue.SmtpServer, emailConfig.CurrentValue.Port, (emailConfig.CurrentValue.Security ? SecureSocketOptions.StartTls : SecureSocketOptions.None));


                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    if (!string.IsNullOrEmpty(emailConfig.CurrentValue.UserName) && !string.IsNullOrEmpty(emailConfig.CurrentValue.Password))
                    {
                        await client.AuthenticateAsync(emailConfig.CurrentValue.UserName, emailConfig.CurrentValue.Password);
                    }
                    if (!string.IsNullOrEmpty(emailConfig.CurrentValue.From))
                    {
                        mailMessage.From.Add(new MailboxAddress(emailConfig.CurrentValue.From, emailConfig.CurrentValue.From));
                    }
                   

                await client.SendAsync(mailMessage);

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                 //   await client.DisconnectAsync(true);
                    client.Dispose();
                }

                return true;
            }
        }
    }
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public List<MailboxAddress> CC { get; set; }
        public List<MailboxAddress> BCC { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string subject, string content)
        {
            To = new List<MailboxAddress>();
            CC = new List<MailboxAddress>();
            BCC = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x,x)));
            CC.AddRange(cc.Select(x => new MailboxAddress(x,x)));
            BCC.AddRange(bcc.Select(x => new MailboxAddress(x, x)));
            Subject = subject;
            Content = content;
        }
    }
}
