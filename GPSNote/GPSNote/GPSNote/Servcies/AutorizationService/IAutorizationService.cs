using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.AutorizationService
{
    public interface IAuthorizationService
    {
        void LogOut();
        bool IsAutorized { get; }
        int IdUser { get; }
    }
}
