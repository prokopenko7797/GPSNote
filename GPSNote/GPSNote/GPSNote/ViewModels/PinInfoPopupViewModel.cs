using Awesomio.Weather.NET.Models.OneCall;
using GPSNote.Extensions;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinShare;
using GPSNote.Servcies.Weather;
using GPSNote.ViewModels.ExtendedViewModels;
using OpenWeatherMap;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSNote.ViewModels
{
    public class PinInfoPopupViewModel : ViewModelBase
    {

        private readonly IWeatherService _weatherService;
        private readonly IPinShareService _pinShareService;

        private PinViewModel pinViewModel;

        public PinInfoPopupViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IWeatherService weatherService, IPinShareService pinShareService)
            : base(navigationService, localizationService)
        {

            _weatherService = weatherService;
            _pinShareService = pinShareService;
        }

        #region -- Public properties --



        private string _PinLatLong;
        public string PinLatLong
        {
            get { return _PinLatLong; }
            set { SetProperty(ref _PinLatLong, value); }
        }

        private string _PinLabel;
        public string PinLabel
        {
            get { return _PinLabel; }
            set { SetProperty(ref _PinLabel, value); }
        }


        private string _PinDescription;
        public string PinDescription
        {
            get { return _PinDescription; }
            set { SetProperty(ref _PinDescription, value); }
        }

        private DelegateCommand<object> _SharePinCommand;
        public DelegateCommand<object> SharePinCommand =>
            _SharePinCommand ?? (_SharePinCommand = new DelegateCommand<object>(OnSharePinCommand));

        private DelegateCommand _PopupCloseCommand;
        public DelegateCommand PopupCloseCommand => 
            _PopupCloseCommand ?? (_PopupCloseCommand = new DelegateCommand(OnPopupCloseAsync));


        string _Temperature;
        public string Temperature
        {
            get { return _Temperature; }
            set { SetProperty(ref _Temperature, value); }
        }

        string _Humidity;
        public string Humidity
        {
            get { return _Humidity; }
            set { SetProperty(ref _Humidity, value); }
        }

        string _Pressure;
        public string Pressure
        {
            get { return _Pressure; }
            set { SetProperty(ref _Pressure, value); }
        }

        string _Wind;
        public string Wind
        {
            get { return _Wind; }
            set { SetProperty(ref _Wind, value); }
        }


        string _Clouds;
        public string Clouds
        {
            get { return _Clouds; }
            set { SetProperty(ref _Clouds, value); }
        }


        string _Precipitation;
        public string Precipitation
        {
            get { return _Precipitation; }
            set { SetProperty(ref _Precipitation, value); }
        }

        string _Weather;
        public string Weather
        {
            get { return _Weather; }
            set { SetProperty(ref _Weather, value); }
        }

        string _LastUpdate;
        public string LastUpdate
        {
            get { return _LastUpdate; }
            set { SetProperty(ref _LastUpdate, value); }
        }


        #endregion

        #region --Overrides--

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);


            if (parameters.TryGetValue<PinViewModel>(nameof(PinViewModel), out var newPinViewModel))
            {
                pinViewModel = newPinViewModel;

                PinLabel = pinViewModel.Label;
                PinLatLong = $"{pinViewModel.Latitude} {pinViewModel.Longitude}";
                PinDescription = pinViewModel.Description;


                OneCallModel oneCallModel = await _weatherService.GetOneCallForecast(newPinViewModel.Latitude,
                                                                                                     newPinViewModel.Longitude);

            }


        }



        #endregion

        #region --Private helpers--

        private void OnSharePinCommand(object sender)
        {
            _pinShareService.SharePinAsync(pinViewModel.ToPinModel());
        }


        private async void OnPopupCloseAsync()
        {
            await NavigationService.GoBackAsync();
        }




        #endregion

    }
}
