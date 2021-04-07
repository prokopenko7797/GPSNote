using GPSNote.Constants;
using GPSNote.Models;
using GPSNote.Servcies.Repository;
using GPSNote.Servcies.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Servcies.PinService
{
    public class PinService : IPinService
    {
        private readonly IRepository _repository;
        private readonly ISettingsManager _Settings;

        public PinService(IRepository repository, 
                          ISettingsManager settingsManager)
        {
            _repository = repository;
            _Settings = settingsManager;

        }

        #region -- IPinService implementation --

        public async Task<bool> EditPinAsync(UserPins userPins)
        {
            return (await _repository.UpdateAsync(userPins) != Constant.SQLError);
        }

        public async Task<bool> AddPinAsync(UserPins userPins)
        {
            userPins.user_id = _Settings.IdUser;
            return (await _repository.InsertAsync(userPins) != Constant.SQLError);
        }

        public async Task<bool> DeletePinAsync(int id)
        {
            if (await _repository.DeleteAsync<UserPins>(id) != Constant.SQLError)
                return true;
            else return false;
        }

        public async Task<UserPins> GetPinByIdAsync(int id)
        {
            return await _repository.GetByIdAsync<UserPins>(id);
        }

        public async Task<IEnumerable<UserPins>> GetUserPinsAsync()
        {
            IEnumerable<UserPins> userPins = await _repository.QueryAsync<UserPins>($"SELECT * FROM {nameof(UserPins)} " +
                $"WHERE user_id='{_Settings.IdUser}'");
       
            return userPins;
        }

        #endregion
    }
}
