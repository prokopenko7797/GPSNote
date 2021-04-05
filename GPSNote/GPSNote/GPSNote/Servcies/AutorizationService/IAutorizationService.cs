﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.AutorizationService
{
    public interface IAuthorizationService
    {
        Task<bool> AuthorizeAsync(string email, string password);
        
        void LogOut();

        int IdUser { get; set; }
        bool IsAutorized { get; }
    }
}
