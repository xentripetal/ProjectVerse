using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Barebones.Logging;
using UnityEngine;

namespace Barebones.MasterServer {
    public class SmtpMailer : Mailer {
        private List<Exception> _sendMailExceptions;
        public string EmailFrom = "YourGame@gmail.com";

        private readonly BmLogger Logger = Msf.Create.Logger(typeof(SmtpMailer).Name);
        public string SenderDisplayName = "Awesome Game";

#if (!UNITY_WEBGL && !UNITY_IOS) || UNITY_EDITOR
        protected SmtpClient SmtpClient;
#endif
        [Header("E-mail settings")] public string SmtpHost = "smtp.gmail.com";

        public string SmtpPassword = "password";
        public int SmtpPort = 587;
        public string SmtpUsername = "username@gmail.com";

        protected virtual void Awake() {
            _sendMailExceptions = new List<Exception>();
            SetupSmtpClient();
        }

        protected virtual void Update() {
            // Log errors for any exceptions that might have occured
            // when sending mail
            if (_sendMailExceptions.Count > 0)
                lock (_sendMailExceptions) {
                    foreach (var exception in _sendMailExceptions) Logger.Error(exception);

                    _sendMailExceptions.Clear();
                }
        }

        protected virtual void SetupSmtpClient() {
#if (!UNITY_WEBGL && !UNITY_IOS) || UNITY_EDITOR
            // Configure mail client
            SmtpClient = new SmtpClient(SmtpHost, SmtpPort);

            // set the network credentials
            SmtpClient.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
            SmtpClient.EnableSsl = true;

            SmtpClient.SendCompleted += (sender, args) => {
                if (args.Error != null)
                    lock (_sendMailExceptions) {
                        _sendMailExceptions.Add(args.Error);
                    }
            };

            ServicePointManager.ServerCertificateValidationCallback =
                delegate { return true; };
#endif
        }

        public override bool SendMail(string to, string subject, string body) {
#if (!UNITY_WEBGL && !UNITY_IOS) || UNITY_EDITOR
            // Create the mail message (from, to, subject, body)
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(EmailFrom, SenderDisplayName);
            mailMessage.To.Add(to);

            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.High;

            // send the mail
            SmtpClient.SendAsync(mailMessage, "");
#endif
            return true;
        }
    }
}