using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.RegistrationService
{
    public interface IRegistrationService
    {
        Task<bool> RegistrateAsync(string name, string email, string password);
    }
}
