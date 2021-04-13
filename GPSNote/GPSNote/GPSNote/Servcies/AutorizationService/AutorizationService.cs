using GPSNote.Models;
using GPSNote.Servcies.Repository;
using GPSNote.Servcies.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GPSNote.Servcies.AutorizationService
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepository _repository;
        private readonly ISettingsManager _settingsManager;


        public AuthorizationService(IRepository repository, ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;

        }

        #region -- IAuthorizationService implementation --

        public bool IsAutorized 
        {
            get { return _settingsManager.IdUser == Constant.NonAuthorized; }
        }

        public int IdUser { get => _settingsManager.IdUser; }

        public void LogOut()
        {
            _settingsManager.IdUser = Constant.NonAuthorized;
        }

        #endregion

    }
}
