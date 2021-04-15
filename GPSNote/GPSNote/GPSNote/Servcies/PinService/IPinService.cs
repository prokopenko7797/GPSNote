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
        Task<bool> DeletePinModelAsync(int id);

        Task<bool> AddPinModelAsync(PinModel profile);

        Task<bool> EditPinModelAsync(PinModel profile);

        Task<PinModel> GetPinModelByIdAsync(int id);

        Task<IEnumerable<PinModel>> GetUserPinsAsync();
    }
}
