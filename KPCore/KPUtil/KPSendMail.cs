/*
 * Copyright 2011-2015 Numeric Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace KPCore.KPUtil
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPSendMail
    {
        public string From { get; set; }
        public string To { get; set; }

        public int Port { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        private bool IsConfigured { get; set; }

        public KPSendMail(string to)
        {
            this.From = "contato@numeric.com.br";
            this.To = to;
        }

        public void Configure(int port, string host, string user, string password)
        {
            this.Port = port;
            this.Host = host;
            this.User = user;
            this.Password = password;

            this.IsConfigured = true;
        }

        public bool SendMail(string subject, string message, bool messageInHtml)
        {
            if (!this.IsConfigured) {
                throw new Exception("It is not configured, please run the 'Configure' method!");
            }

            MailMessage mail = new MailMessage(this.From, this.To);
            mail.Priority = MailPriority.High;
            mail.IsBodyHtml = messageInHtml;
            mail.Subject = subject;
            mail.Body = message;


            NetworkCredential basicCredential = new NetworkCredential(this.User, this.Password); 

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Port = this.Port;
            client.Host = this.Host;
            client.Credentials = basicCredential;
            client.EnableSsl = true;

            try
            {
                client.Send(mail);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }
    }
}
