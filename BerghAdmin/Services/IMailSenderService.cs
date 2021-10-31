using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BerghAdmin.Services
{
    public interface IMailSenderService
    {
        Task SendMailAsync(MailAddress from, IEnumerable<MailAddress> to, Stream mailBody);
    }
}
