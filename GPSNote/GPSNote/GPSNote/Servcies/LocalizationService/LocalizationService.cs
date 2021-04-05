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

        #region -- IterfaceName implementation --

        private readonly ISettingsManager _settingsManager;

        #endregion


        private readonly ResourceManager _ResourceManager;
        private CultureInfo _CurrentCultureInfo;



        public LocalizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;


            _CurrentCultureInfo = new CultureInfo(_settingsManager.Lang);
            _ResourceManager = new ResourceManager(typeof(LocalizationResource));

            MessagingCenter.Subscribe<object, CultureInfo>(this,
                string.Empty, OnCultureChanged);
        }


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


        #region _____Private Helpers______

        private void OnCultureChanged(object s, CultureInfo ci)
        {
            _CurrentCultureInfo = ci;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
