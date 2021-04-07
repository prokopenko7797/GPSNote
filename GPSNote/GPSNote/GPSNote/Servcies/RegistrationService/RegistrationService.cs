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

        private readonly IRepository _repository;

        public RegistrationService(IRepository repository)
        {
            _repository = repository;
        }


        #region --  IRegistrationService implementation --

        public async Task<bool> RegistrateAsync(User user)
        {
            bool result = true;

            User registredUser = await _repository.FindWithQueryAsync<User>($"SELECT * FROM {nameof(User)} WHERE Email='{user.Email}'");

            if (registredUser != null)
            {
                result = false;
            }
            else
            {
                await _repository.InsertAsync(user);
            }

            return result;
        }

        #endregion
    }
}
