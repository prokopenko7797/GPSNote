using GPSNote.Models;
using GPSNote.ViewModels.ExtendedViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Servcies.PinShare
{
    public interface IPinShareService
    {
        void SharePinAsync(PinModel pinModel);
    }
}
