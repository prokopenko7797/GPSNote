using GPSNote.Constants;
using GPSNote.Models;
using GPSNote.Servcies.Repository;
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


        public AuthorizationService(IRepository repository)
        {
            _repository = repository;

        }

        #region -- IAuthorizationService implementation --

        #region -- Public properties --

        public bool IsAutorized 
        {
            get { return IdUser == Constant.NonAuthorized; }
        }


        public void LogOut()
        {
            IdUser = Constant.NonAuthorized;
        }

        public int IdUser
        {
            get => Preferences.Get(nameof(IdUser), Constant.NonAuthorized);
            set => Preferences.Set(nameof(IdUser), value);
        }

        #endregion

        public async Task<bool> AuthorizeAsync(string email, string password)
        {
            bool result = false;

            var user = await _repository.FindWithQueryAsync<User>($"SELECT * FROM {nameof(User)} WHERE Email='{email}' AND Password='{password}'");

            if (user != null)
            {
                IdUser = user.id;
                result = true;
            }

            return result;
        }

        #endregion

    }
}
