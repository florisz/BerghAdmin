using System;
using System.Threading.Tasks;

namespace BerghAdmin.Services
{
    public interface ISendMailService
    {
        Task SendMail();
    }
}
