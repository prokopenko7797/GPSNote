using GPSNote.Models;
using GPSNote.Servcies.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.RegistrationService
{
    public class RegistrationService : IRegistrationService
    {
        #region _____Services______
        private readonly IRepository _repository;
        #endregion

        public RegistrationService(IRepository repository)
        {
            _repository = repository;
        }


        #region ______Public Methods______
        public async Task<bool> RegistrateAsync(string name, string email, string password)
        {
            bool result = false;

            User user1 = await _repository.FindWithQueryAsync<User>($"SELECT * FROM {nameof(User)} WHERE email='{email}'");

            if (user1 !=null)
            {

                return false;
            }

            await _repository.InsertAsync(new User { Name = name, Email = email, Password = password });
            return true;
        }

        #endregion
    }
}
