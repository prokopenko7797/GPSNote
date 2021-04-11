using GPSNote.Servcies.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Servcies.LastPositionService
{
    public class LastPositionService : ILastPositionService
    {
        private ISettingsManager _settingsManager;
        public LastPositionService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public double Latitude 
        {
            get => _settingsManager.Latitude;
            set => _settingsManager.Latitude = value;
        }
        public double Longitude 
        {
            get => _settingsManager.Longitude;
            set => _settingsManager.Longitude = value;
        }
        public double Zoom 
        {
            get => _settingsManager.Zoom;
            set => _settingsManager.Zoom = value;
        }
    }
}

