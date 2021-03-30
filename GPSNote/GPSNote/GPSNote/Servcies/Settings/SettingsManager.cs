using GPSNote.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GPSNote.Servcies.Settings
{
    public class SettingsManager : ISettingsManager
    {
        #region ______Public Methods______


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
        #endregion
    }
}
