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

        private readonly ISettingsManager  _settings;

        private readonly ResourceManager _ResourceManager;
        private CultureInfo _CurrentCultureInfo;


        public LocalizationService(ISettingsManager settings)
        {
            _settings = settings;
            _CurrentCultureInfo = new CultureInfo(_settings.Lang);
            _ResourceManager = new ResourceManager(typeof(LocalizationResource));

            MessagingCenter.Subscribe<object, CultureInfo>(this,
                string.Empty, OnCultureChanged);
        }

        #region -- ILocalizationService implementation --

        public string this[string key]
        {
            get
            {
                return _ResourceManager.GetString(key, _CurrentCultureInfo);
            }
        }

        public void ChangeCulture(string lang)
        {
            MessagingCenter.Send<object, CultureInfo>(this, string.Empty, new CultureInfo(lang));
        }

        public string Lang 
        {
            get => _settings.Lang; 
            set => _settings.Lang = value;
        }

        #endregion

        #region -- INotifyPropertyChanged implementation --

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region -- Private helpers --

        private void OnCultureChanged(object s, CultureInfo ci)
        {
            _CurrentCultureInfo = ci;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
        }

        #endregion


    }
}
