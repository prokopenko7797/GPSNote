using GPSNote.Servcies.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Servcies.ThemeService
{
    public class ThemeService : IThemeService
    {
        private ISettingsManager _settingsManager; 
        public ThemeService(ISettingsManager settingsManager) 
        {
            _settingsManager = settingsManager;
        }
        public int Theme 
        {
            get => _settingsManager.Theme; 
            set => _settingsManager.Theme = value; 
        }
    }
}
