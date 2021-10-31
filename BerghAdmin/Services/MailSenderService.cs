using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BerghAdmin.Services
{
    public class MailAddress
    {
        string EmailAddres { get; set;}
        string Name { get; set;}
    }

    public class MailSenderService : IMailSenderService
    {
        public Task SendMailAsync(MailAddress from, IEnumerable<MailAddress> to, Stream mailBody)
        {
            throw new NotImplementedException();
        }
    }
}
