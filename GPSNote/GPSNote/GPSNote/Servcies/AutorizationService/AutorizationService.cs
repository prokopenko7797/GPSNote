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
        #region ______Services_________
        private readonly IRepository _repository;

        #endregion

        public AuthorizationService(IRepository repository)
        {
            _repository = repository;

        }

        #region ______Public Methods______

        public async Task<bool> AuthorizeAsync(string email, string password)
        {

            var user = await _repository.FindWithQueryAsync<User>($"SELECT * FROM {nameof(User)} WHERE email='{email}' AND password='{password}'");

            if (user != null)
            {
                IdUser = user.id;
                return true;
            }

            return false;
        }

        public bool AuthorizeCheck()
        {
            if (IdUser == Constant.NonAuthorized) return true;
            else return false;
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
    }
}
