using GPSNote.CustomControls.CustomMap;
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
using GPSNote.Servcies.LastPositionService;
using System.Threading.Tasks;
using System.ComponentModel;
using GPSNote.Views;

namespace GPSNote.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;
        private readonly IPinService _pinService;
        private readonly ILastPositionService _lastPositionService;

        public MapPageViewModel(INavigationService navigationService, ILocalizationService localizationService, 
            IPageDialogService pageDialogService, IPinService pinService, ILastPositionService lastPositionService)
            : base(navigationService, localizationService)
        {
            _pageDialogService = pageDialogService;
            _pinService = pinService;
            _lastPositionService = lastPositionService;



        }

        #region -- Public properties --
        

        public DelegateCommand<object> PinClickedCommand => new DelegateCommand<object>(PinClicked);

        private async void PinClicked(object p)
        {
            Pin SelectedPin = (Pin)p;

            int i = PinList.IndexOf(SelectedPin);

            await _pageDialogService.DisplayAlertAsync("asdasd", PinObs[i].Label, "Ok");

            IsPinTapped = true;
            PinLabel = PinObs[i].Label;
            PinDescription = PinObs[i].Description;
            PinLatitude = PinObs[i].Latitude;
            PinLongitude = PinObs[i].Longitude;

        }

        public DelegateCommand<object> ItemTappedCommand => new DelegateCommand<object>(TapCommand);

        private async void TapCommand(object p)
        {
            Position position = (Position)p;

            //await _pageDialogService.DisplayAlertAsync("It's", $"{position.Latitude} {position.Longitude}", "Ok");

            Pin pin = new Pin()
            {
                Type = PinType.Place,
                Label = "Central Park NYC",
                Address = "New York City, NY 10022",
                Position = position,
                Tag = "id_new_york"
                
                
            };

            await _pinService.AddPinAsync(new UserPins(){ Latitude = position.Latitude, Longitude = position.Longitude, Label = "My pin", IsEnabled = true});

            PinList = GetPins(await _pinService.GetUserPinsAsync());

        }

        public DelegateCommand<object> CameraIdLedCommand => new DelegateCommand<object>(OnCameraMovedAsync);


        private void OnCameraMovedAsync(object position)
        {
            CameraPosition cameraPosition = position as CameraPosition;

            //_lastPositionService.Latitude = cameraPosition.Target.Latitude;
            //_lastPositionService.Longitude = cameraPosition.Target.Longitude;
            //_lastPositionService.Zoom = cameraPosition.Zoom;
        }


        public DelegateCommand<object> MoveToCommand => new DelegateCommand<object>(MoveToc);


        private void MoveToc(object position)
        {
            MoveTo = new MapSpan(new Position(3, 3), 3, 3).WithZoom(10);


        }

        private bool _IsPinTapped;

        public bool IsPinTapped
        {
            get { return _IsPinTapped; }
            set { SetProperty(ref _IsPinTapped, value); }
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

        private MapType _type;

        public MapType type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private List<Pin> _pins = new List<Pin>();

        public List<Pin> PinList
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        private ObservableCollection<UserPins> _pinList;
        public ObservableCollection<UserPins> PinObs
        {
            get { return _pinList; }
            set { SetProperty(ref _pinList, value); }
        }

        private UserPins _SelectedItem;
        public UserPins SelectedItem
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


        #endregion

        #region --Private helpers--
        private List<Pin> GetPins(IEnumerable<UserPins> userPins)
        {
            List<Pin> pins = new List<Pin>();

            foreach (UserPins p in userPins)
            {
                Position position = new Position(p.Latitude, p.Longitude);

                pins.Add(new Pin() { Position = position, Label = p.Label, IsVisible = p.IsEnabled });
            }
            return pins;
        }

        #endregion

        #region --Overrides--

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            PinList = GetPins( await _pinService.GetUserPinsAsync());
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue<ObservableCollection<UserPins>>(nameof(PinObs), out var newPinsValue))
            {
                PinObs = newPinsValue;

                foreach (var item in PinObs)
                {
                    PinList.Add(item.ToPin());
                }
            }

            if (parameters.TryGetValue<UserPins>(nameof(SelectedItem), out var newSelectedItem))
            {
                SelectedItem = newSelectedItem;
            }


        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            parameters.Add(nameof(this.PinList), this.PinList);
        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedItem) )
            {
                MoveTo = new MapSpan(new Position(SelectedItem.Latitude, SelectedItem.Longitude), 1, 1).WithZoom(10);
            }
        }

        #endregion
    }
}
