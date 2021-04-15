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

        public async Task<bool> EditPinModelAsync(PinModel userPins)
        {
            return (await _repository.UpdateAsync(userPins) != Constant.SQLError);
        }

        public async Task<bool> AddPinModelAsync(PinModel userPins)
        {
            userPins.user_id = _Settings.IdUser;

            return await _repository.InsertAsync(userPins) != Constant.SQLError;
        }

        public async Task<bool> DeletePinModelAsync(int id)
        {
            return await _repository.DeleteAsync<PinModel>(id) != Constant.SQLError;
        }

        public async Task<PinModel> GetPinModelByIdAsync(int id)
        {
            return await _repository.GetByIdAsync<PinModel>(id);
        }

        public async Task<IEnumerable<PinModel>> GetUserPinsAsync()
        {
            IEnumerable<PinModel> userPins = await _repository.QueryAsync<PinModel>($"SELECT * FROM {nameof(PinModel)} " +
                $"WHERE user_id='{_Settings.IdUser}'");
       
            return userPins;
        }

        #endregion
    }
}
