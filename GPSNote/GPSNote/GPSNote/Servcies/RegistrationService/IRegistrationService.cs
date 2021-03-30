using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.RegistrationService
{
    public interface IRegistrationService
    {
        Task<bool> RegistrateAsync(string login, string password);
    }
}
