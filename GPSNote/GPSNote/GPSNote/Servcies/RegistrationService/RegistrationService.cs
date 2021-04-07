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


        #region --  IRegistrationService implementation --

        public async Task<bool> RegistrateAsync(User user)
        {
            bool result = false;

            User user1 = await _repository.FindWithQueryAsync<User>($"SELECT * FROM {nameof(User)} WHERE Email='{user.Email}'");

            if (user1 !=null)
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
