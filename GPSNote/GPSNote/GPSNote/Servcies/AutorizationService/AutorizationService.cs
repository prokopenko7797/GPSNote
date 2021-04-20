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
        private readonly IRepositoryService _repository;
        private readonly ISettingsManager _settingsManager;

        public AuthorizationService(IRepositoryService repository, ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;

        }

        #region -- IAuthorizationService implementation --

        public bool IsAuthorized 
        {
            get { return _settingsManager.UserId == Constant.NonAuthorized; }
        }

        public int IdUser { get => _settingsManager.UserId; }

        public void LogOut()
        {
            _settingsManager.UserId = Constant.NonAuthorized;
        }

        #endregion

    }
}
