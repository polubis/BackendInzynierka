using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inzynierka.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAfterRegister(string EmailAdress, string GeneratedLink, string Subject, string Username, string operationMessage, string operationLink);
    }
}
