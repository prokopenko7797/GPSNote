using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Servcies.PinService
{
    public interface IPinService
    {
        Task<bool> DeleteAsync(int id);
        Task<bool> AddAsync(UserPins profile);

        Task<bool> EditAsync(UserPins profile);

        Task<UserPins> GetByIdAsync(int id);
        Task<List<Pin>> GetUserPinsAsync();
    }
}
