using GPSNote.Servcies.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GPSNote.Servcies.ThemeService
{
    public class ThemeService : IThemeService
    {
        private ISettingsManager _settingsManager; 
        public ThemeService(ISettingsManager settingsManager) 
        {
            _settingsManager = settingsManager;
        }


        public OSAppTheme GetCurrentTheme()
        {
            return (OSAppTheme)_settingsManager.Theme;
        }

        public void SetTheme(OSAppTheme theme)
        {
            _settingsManager.Theme = (int)theme;
            Application.Current.UserAppTheme = theme;
        }
    }
}
