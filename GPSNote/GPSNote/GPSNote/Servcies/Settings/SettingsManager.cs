using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Servcies.Settings
{
    public class SettingsManager : ISettingsManager
    {

        #region -- ISettingsManager implementation --

        public string Lang
        {
            get => Preferences.Get(nameof(Lang), Constant.DefaultLanguage);
            set => Preferences.Set(nameof(Lang), value);
        }

        public int Theme
        {
            get => Preferences.Get(nameof(Theme), Constant.DefaultTheme);
            set => Preferences.Set(nameof(Theme), value);
        }

        public int UserId
        {
            get => Preferences.Get(nameof(UserId), Constant.NonAuthorized);
            set => Preferences.Set(nameof(UserId), value);
        }

        public double Latitude
        {
            get => Preferences.Get(nameof(Latitude), 41.8899999412824);
            set => Preferences.Set(nameof(Latitude), value);
        }
        public double Longitude
        {
            get => Preferences.Get(nameof(Longitude), 12.489999797344);
            set => Preferences.Set(nameof(Longitude), value);
        }
        public double Zoom
        {
            get => Preferences.Get(nameof(Zoom), 10);
            set => Preferences.Set(nameof(Zoom), value);
        }



        #endregion
    }
}
