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

        Task<bool> AddPinAsync(PinModel profile);

        Task<bool> EditPinAsync(PinModel profile);

        Task<PinModel> GetPinByIdAsync(int id);

        Task<IEnumerable<PinModel>> GetUserPinsAsync();
    }
}
