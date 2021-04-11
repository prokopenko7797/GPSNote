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

        private ObservableCollection<UserPins> _Current;

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



        public DelegateCommand<object> ItemTappedCommand => new DelegateCommand<object>(TapCommand);

        private void TapCommand(object p)
        {
            IsListViewVisible = false;
            IsPinTapped = false;

        }



        public DelegateCommand<object> MoveToCommand => new DelegateCommand<object>(MoveToc);


        private void MoveToc(object position)
        {
            Position newPosition = new Position(SelectedItem.Latitude, SelectedItem.Longitude);
            MoveTo = new MapSpan(newPosition, 3, 3).WithZoom(10);


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

        public DelegateCommand<object> OnTextChangedCommand => new DelegateCommand<object>(TextChangedCommand);
        public DelegateCommand<object> OnFocusedCommand => new DelegateCommand<object>(FocusedCommand);

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


        private void TextChangedCommand(object sender)
        {
            if (string.IsNullOrWhiteSpace(SearchBarText))
            {
                PinObs = _Current;
            }
            else
            {
                PinObs = new ObservableCollection<UserPins>(PinObs.Where(pin => pin.Label.Contains(SearchBarText)));
                
            }
        }

        private void FocusedCommand(object sender)
        {
            IsListViewVisible = true;
        }


        private void PinClicked(object p)
        {
            Pin SelectedPin = (Pin)p;

            int i = PinList.IndexOf(SelectedPin);

            IsPinTapped = true;
            PinLabel = PinObs[i].Label;
            PinDescription = PinObs[i].Description;
            PinLatitude = PinObs[i].Latitude;
            PinLongitude = PinObs[i].Longitude;

        }

        #endregion

        #region --Overrides--

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            PinList = GetPins( await _pinService.GetUserPinsAsync());
            PinObs = new ObservableCollection<UserPins>(await _pinService.GetUserPinsAsync());
            _Current = PinObs;
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue<ObservableCollection<UserPins>>(nameof(PinObs), out var newPinsValue))
            {
                PinObs = newPinsValue;
                _Current = newPinsValue;

                List<Pin> nPin = new List<Pin>();

                foreach (var item in PinObs)
                {
                    nPin.Add(item.ToPin());
                }

                PinList = nPin;
            }

            if (parameters.TryGetValue<UserPins>(nameof(SelectedItem), out var newSelectedItem))
            {
                SelectedItem = newSelectedItem;
            }


        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedItem) )
            {
                MoveTo = new MapSpan(new Position(SelectedItem.Latitude, SelectedItem.Longitude), 1, 1).WithZoom(10);
                IsListViewVisible = false;
            }
        }

        

        #endregion
    }
}
