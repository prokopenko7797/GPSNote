using GPSNote.Models;
using GPSNote.Servcies.Repository;
using GPSNote.Servcies.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository _repository;
        private readonly ISettingsManager _settingsManager;


        public AuthenticationService(IRepository repository, ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;

        }

        #region -- IAuthenticationService implementation --
        
        public async Task<bool> SignInAsync(string email, string password)
        {
            bool result = false;

            var user = await _repository.FindWithQueryAsync<User>($"SELECT * FROM {nameof(User)} WHERE Email='{email}' AND Password='{password}'");

            if (user != null)
            {
                _settingsManager.IdUser = user.id;
                result = true;
            }

            return result;
        }


        public async Task<bool> SignUpAsync(string name, string email, string password)
        {
            bool result = false;

            User registredUser = await _repository.FindWithQueryAsync<User>($"SELECT * FROM {nameof(User)} WHERE Email='{email}'");

            if (registredUser == null)
            {
                await _repository.InsertAsync(new User { Name = name, Email = email, Password = password });
            }
            else
            {
                result = false;
            }

            return result;
        }

        #endregion
    }
}
