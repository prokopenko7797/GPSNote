using GPSNote.CustomControls;
using GPSNote.Models;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
using GPSNote.Servcies.Settings;
using GPSNote.ViewModels.ExtendedViewModels;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using GPSNote.Extensions;
using System.Threading.Tasks;
using System.ComponentModel;
using GPSNote.Views;
using OpenWeatherMap;
using GPSNote.Servcies.Weather;

namespace GPSNote.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;
        private readonly IPinService _pinService;
        private readonly IWeatherService _weatherService;


        public MapPageViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IPageDialogService pageDialogService, IPinService pinService, IWeatherService weatherService)
            : base(navigationService, localizationService)
        {
            _pageDialogService = pageDialogService;
            _pinService = pinService;
            _weatherService = weatherService;
        }

        #region -- Public properties --

        private bool _IsPinTapped;

        public bool IsPinTapped
        {
            get { return _IsPinTapped; }
            set { SetProperty(ref _IsPinTapped, value); }
        }

        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }


        private double _PinLatitude;
        public double PinLatitude
        {
            get { return _PinLatitude; }
            set { SetProperty(ref _PinLatitude, value); }
        }

        private double _PinLongitude;
        public double PinLongitude
        {
            get { return _PinLongitude; }
            set { SetProperty(ref _PinLongitude, value); }
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


        private ObservableCollection<PinViewModel> _ControlObs;

        public ObservableCollection<PinViewModel> ControlObs
        {
            get { return _ControlObs; }
            set { SetProperty(ref _ControlObs, value); }
        }

        private ObservableCollection<PinViewModel> _pinList;
        public ObservableCollection<PinViewModel> PinObs
        {
            get { return _pinList; }
            set { SetProperty(ref _pinList, value); }
        }

        private PinViewModel _SelectedItem;
        public PinViewModel SelectedListItem
        {
            get { return _SelectedItem; }
            set { SetProperty(ref _SelectedItem, value); }
        }


        MapSpan _MoveTo;
        public MapSpan MoveTo
        {
            get { return _MoveTo; }
            set { SetProperty(ref _MoveTo, value); }
        }

        string _SearchBarText;
        public string SearchBarText
        {
            get { return _SearchBarText; }
            set { SetProperty(ref _SearchBarText, value); }
        }

        private bool _IsListViewVisible;

        public bool IsListViewVisible
        {
            get { return _IsListViewVisible; }
            set { SetProperty(ref _IsListViewVisible, value); }
        }

        private int _HeightRequest;
        public int HeightRequest
        {
            get { return _HeightRequest; }
            set { SetProperty(ref _HeightRequest, value); }
        }

        private DelegateCommand<object> _TextChangedCommand;
        public DelegateCommand<object> TextChangedCommand =>
            _TextChangedCommand ?? (_TextChangedCommand = new DelegateCommand<object>(OnTextChangedCommand));


        private DelegateCommand<object> _FocusedCommand;
        public DelegateCommand<object> FocusedCommand =>
            _FocusedCommand ?? (_FocusedCommand = new DelegateCommand<object>(OnFocusedCommand));


        private DelegateCommand<object> _PinClickedCommand;
        public DelegateCommand<object> PinClickedCommand =>
            _PinClickedCommand ?? (_PinClickedCommand = new DelegateCommand<object>(OnPinClicked));


        private DelegateCommand<object> _ItemTappedCommand;
        public DelegateCommand<object> ItemTappedCommand =>
            _ItemTappedCommand ?? (_ItemTappedCommand = new DelegateCommand<object>(OnItemTappedCommand));


        private DelegateCommand<object> _MoveToCommand;
        public DelegateCommand<object> MoveToCommand =>
            _MoveToCommand ?? (_MoveToCommand = new DelegateCommand<object>(OnMoveToCommand));


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

        #region --Private helpers--



        private void OnItemTappedCommand(object p)
        {
            IsListViewVisible = false;
            IsPinTapped = false;
        }

        private void OnMoveToCommand(object position)
        {
            Position newPosition = new Position(SelectedListItem.Latitude, SelectedListItem.Longitude);
            Random zoom = new Random();

            MoveTo = new MapSpan(newPosition, 3, 3).WithZoom(10 + zoom.NextDouble());
        }



        private void OnTextChangedCommand(object sender)
        {
            if (string.IsNullOrWhiteSpace(SearchBarText))
            {
                PinObs = ControlObs;
            }
            else
            {
                string low = SearchBarText.ToLower();

                PinObs = new ObservableCollection<PinViewModel>(ControlObs.Where(pin => (pin.Label.ToLower()).Contains(low) ||
                                                                                  (pin.Description.ToLower()).Contains(low) ||
                                                                                  (pin.Latitude.ToString()).Contains(low) ||
                                                                                  (pin.Longitude.ToString()).Contains(low)));
            }
            ChangeHeight();
        }

        private void ChangeHeight() 
        {
            if (PinObs.Count() < 4)
            {
                HeightRequest = PinObs.Count() * 50;
            }
            else
            {
                HeightRequest = 200;
            }
        }


        private void OnFocusedCommand(object sender)
        {
            IsListViewVisible = true;
            ChangeHeight();
        }

        private void OnPinClicked(object sender)
        {
            Pin SelectedPin = (Pin)sender;

            DisplayInfoPinViewModel(ControlObs.Where(pinView => pinView.Label.Contains(SelectedPin.Label)).First());
        }


        private async void DisplayInfoPinViewModel(PinViewModel pinView) 
        {
            IsPinTapped = true;
            PinLabel = pinView.Label;
            PinDescription = pinView.Description;
            PinLatitude = pinView.Latitude;
            PinLongitude = pinView.Longitude;


            CurrentWeatherResponse currentWeather = await _weatherService.GetCurrentWeatherAsync(pinView.Latitude, 
                                                                                                 pinView.Longitude);
 
            Temperature = currentWeather.Temperature.Value.ToString() + 
                          currentWeather.Temperature.Unit;

            Humidity = currentWeather.Humidity.Value.ToString();
            Pressure = currentWeather.Pressure.Value.ToString() + 
                       currentWeather.Pressure.Unit; 

            Wind = currentWeather.Wind.Speed.Value.ToString() + 
                   currentWeather.Wind.Speed.Name + 
                   currentWeather.Wind.Speed.Value.ToString() + 
                   currentWeather.Wind.Direction.Value.ToString() + 
                   currentWeather.Wind.Direction.Value.ToString();

            Clouds = currentWeather.Clouds.Value.ToString() + 
                     currentWeather.Clouds.Name;

            Precipitation = currentWeather.Precipitation.Value.ToString() + 
                            currentWeather.Precipitation.Unit + 
                            currentWeather.Precipitation.Mode;

            Weather = currentWeather.Weather.Value.ToString();
            LastUpdate = currentWeather.LastUpdate.Value.ToString();
        }

        #endregion

        #region --Overrides--

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            PinObs = new ObservableCollection<PinViewModel>((await _pinService.GetUserPinsAsync()).ToOpsOfPinView());
            ControlObs = PinObs;
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue<ObservableCollection<PinViewModel>>(nameof(PinViewModel), out var newPinsValue))
            {
                PinObs = newPinsValue;
                ControlObs = newPinsValue;

            }

            if (parameters.TryGetValue<PinViewModel>(nameof(SelectedListItem), out var newSelectedItem))
            {
                SelectedListItem = newSelectedItem;
            }

            bool IsUpdated = false;

            if (parameters.TryGetValue<bool>(nameof(IsUpdated), out var newUpdate))
            {
                if (newUpdate)
                {
                    PinObs = new ObservableCollection<PinViewModel>((await _pinService.GetUserPinsAsync()).ToOpsOfPinView());
                    ControlObs = PinObs;
                }
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedListItem) )
            {
                MoveTo = new MapSpan(new Position(SelectedListItem.Latitude, SelectedListItem.Longitude), 1, 1).WithZoom(10);
                IsListViewVisible = false;
                IsSelected = SelectedListItem != null;

                DisplayInfoPinViewModel(ControlObs.Where(pinView => pinView.Label.Contains(SelectedListItem.Label)).First());

            }
        }

        #endregion
    }
}
