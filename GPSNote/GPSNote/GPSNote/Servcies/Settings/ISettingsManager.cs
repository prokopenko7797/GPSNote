using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Servcies.Settings
{
    public interface ISettingsManager
    {
        string Lang { get; set; }

        int Theme { get; set; }

        int IdUser { get; set; }

        double Latitude { get; set; }

        double Longitude { get; set; }

        double Zoom { get; set; }

    }
}
