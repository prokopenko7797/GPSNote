using GPSNote.Resources;
using GPSNote.Servcies.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Text;
using Xamarin.Forms;

namespace GPSNote.Servcies.LocalizationService
{
    public class LocalizationService : ILocalizationService, INotifyPropertyChanged
    {


        #region _____Services______

        private readonly ISettingsManager _settingsManager;

        #endregion

        #region _____Private______

        private readonly ResourceManager ResourceManager;
        private CultureInfo CurrentCultureInfo;

        #endregion

        public LocalizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;


            CurrentCultureInfo = new CultureInfo(_settingsManager.Lang);
            ResourceManager = new ResourceManager(typeof(LocalizationResource));

            MessagingCenter.Subscribe<object, CultureInfo>(this,
                string.Empty, OnCultureChanged);
        }

        #region _____Public Methods______

        public string this[string key]
        {
            get
            {
                return ResourceManager.GetString(key, CurrentCultureInfo);
            }
        }


        public void CultureChange(string lang)
        {

            MessagingCenter.Send<object, CultureInfo>(this, string.Empty, new CultureInfo(lang));
        }

        #endregion

        #region _____Private Helpers______

        private void OnCultureChanged(object s, CultureInfo ci)
        {
            CurrentCultureInfo = ci;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
