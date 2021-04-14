using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> SignUpAsync(string name, string email, string password);

        Task<bool> SignInAsync(string email, string password);
    }
}
