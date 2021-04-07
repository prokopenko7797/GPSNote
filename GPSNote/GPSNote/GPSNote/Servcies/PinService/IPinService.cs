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
        Task<bool> DeletePinAsync(int id);
        Task<bool> AddPinAsync(UserPins profile);
        Task<bool> EditPinAsync(UserPins profile);
        Task<UserPins> GetPinByIdAsync(int id);
        Task<IEnumerable<UserPins>> GetUserPinsAsync();
    }
}
