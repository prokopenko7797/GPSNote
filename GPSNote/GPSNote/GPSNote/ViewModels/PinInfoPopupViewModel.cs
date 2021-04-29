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
using Xamarin.Forms;

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


        private string _Day0;
        public string Day0
        {
            get { return _Day0; }
            set { SetProperty(ref _Day0, value); }
        }
        private string _Day1;
        public string Day1
        {
            get { return _Day1; }
            set { SetProperty(ref _Day1, value); }
        }
        private string _Day2;
        public string Day2
        {
            get { return _Day2; }
            set { SetProperty(ref _Day2, value); }
        }
        private string _Day3;
        public string Day3
        {
            get { return _Day3; }
            set { SetProperty(ref _Day3, value); }
        }
        private string _MaxMinTemp0;
        public string MaxMinTemp0
        {
            get { return _MaxMinTemp0; }
            set { SetProperty(ref _MaxMinTemp0, value); }
        }
        private string _MaxMinTemp1;
        public string MaxMinTemp1
        {
            get { return _MaxMinTemp1; }
            set { SetProperty(ref _MaxMinTemp1, value); }
        }
        private string _MaxMinTemp2;
        public string MaxMinTemp2
        {
            get { return _MaxMinTemp2; }
            set { SetProperty(ref _MaxMinTemp2, value); }
        }
        private string _MaxMinTemp3;
        public string MaxMinTemp3
        {
            get { return _MaxMinTemp3; }
            set { SetProperty(ref _MaxMinTemp3, value); }
        }
        private ImageSource _WeatherIcon0;
        public ImageSource WeatherIcon0
        {
            get { return _WeatherIcon0; }
            set { SetProperty(ref _WeatherIcon0, value); }
        }
        private ImageSource _WeatherIcon1;
        public ImageSource WeatherIcon1
        {
            get { return _WeatherIcon1; }
            set { SetProperty(ref _WeatherIcon1, value); }
        }
        private ImageSource _WeatherIcon2;
        public ImageSource WeatherIcon2
        {
            get { return _WeatherIcon2; }
            set { SetProperty(ref _WeatherIcon2, value); }
        }
        private ImageSource _WeatherIcon3;
        public ImageSource WeatherIcon3
        {
            get { return _WeatherIcon3; }
            set { SetProperty(ref _WeatherIcon3, value); }
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
                PinLatLong = $"{pinViewModel.Latitude}, {pinViewModel.Longitude}";
                PinDescription = pinViewModel.Description;


                OneCallModel oneCallModel = await _weatherService.GetOneCallForecast(newPinViewModel.Latitude, newPinViewModel.Longitude);

                MaxMinTemp0 = $"{(int)oneCallModel.Daily[0].Temp.Max} ° {(int)oneCallModel.Daily[0].Temp.Min} °";
                MaxMinTemp1 = $"{(int)oneCallModel.Daily[1].Temp.Max} ° {(int)oneCallModel.Daily[1].Temp.Min} °";
                MaxMinTemp2 = $"{(int)oneCallModel.Daily[2].Temp.Max} ° {(int)oneCallModel.Daily[2].Temp.Min} °";
                MaxMinTemp3 = $"{(int)oneCallModel.Daily[3].Temp.Max} ° {(int)oneCallModel.Daily[3].Temp.Min} °";

                WeatherIcon0 = $"http://openweathermap.org/img/wn/{oneCallModel.Daily[0].Weather[0].Icon}@2x.png";
                WeatherIcon1 = $"http://openweathermap.org/img/wn/{oneCallModel.Daily[1].Weather[0].Icon}@2x.png";
                WeatherIcon2 = $"http://openweathermap.org/img/wn/{oneCallModel.Daily[2].Weather[0].Icon}@2x.png";
                WeatherIcon3 = $"http://openweathermap.org/img/wn/{oneCallModel.Daily[3].Weather[0].Icon}@2x.png";

                List<string> ld = new List<string>();

                for (int i = 0; i < 4; i++)
                {
                    DateTime dt = UnixTimeStampToDateTime(oneCallModel.TimezoneOffset + oneCallModel.Daily[i].Dt);

                    DayOfWeek d = dt.DayOfWeek;
                    ld.Add(d.ToString().Substring(0, 3));
                }

                Day0 = ld[0];
                Day1 = ld[1];
                Day2 = ld[2];
                Day3 = ld[3];

            }


        }



        #endregion

        #region --Private helpers--




        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }


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
