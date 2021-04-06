using GPSNote.Constants;
using GPSNote.Models;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Servcies.PinService
{
    public class PinService : IPinService
    {

        #region ______Services_________
        private readonly IRepository _repository;
        private readonly IAuthorizationService _authorizationService;

        #endregion

        public PinService(IRepository repository, IAuthorizationService authorizationService)
        {
            _repository = repository;
            _authorizationService = authorizationService;

        }

        public async Task<bool> EditAsync(UserPins userPins)
        {
            return (await _repository.UpdateAsync(userPins) != Constant.SQLError);
        }

        public async Task<bool> AddAsync(UserPins userPins)
        {
            userPins.user_id = _authorizationService.IdUser;
            return (await _repository.InsertAsync(userPins) != Constant.SQLError);
        }



        public async Task<bool> DeleteAsync(int id)
        {
            if (await _repository.DeleteAsync<UserPins>(id) != Constant.SQLError)
                return true;
            else return false;
        }

        public async Task<UserPins> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync<UserPins>(id);
        }

        public async Task<List<Pin>> GetUserPinsAsync()
        {
            IEnumerable<UserPins> userPins = await _repository.QueryAsync<UserPins>($"SELECT * FROM {nameof(UserPins)} " +
                $"WHERE user_id='{_authorizationService.IdUser}'");
            List<Pin> pins = new List<Pin>();

     


            foreach (UserPins p in userPins)
            {
                Position position = new Position(p.Latitude, p.Longitude);

                pins.Add(new Pin() {Position = position, Label = p.Label, Tag = p.Tag });
            }
            return pins;

        }



   
    }
}
