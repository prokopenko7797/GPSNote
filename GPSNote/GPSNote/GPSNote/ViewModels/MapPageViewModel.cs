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

        private ObservableCollection<PinViewModel> _Current;

        public MapPageViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IPageDialogService pageDialogService, IPinService pinService, ILastPositionService lastPositionService)
            : base(navigationService, localizationService)
        {
            _pageDialogService = pageDialogService;
            _pinService = pinService;
            _lastPositionService = lastPositionService;



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




        private List<Pin> _pins = new List<Pin>();

        public List<Pin> PinList
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        private ObservableCollection<PinViewModel> _pinList;
        public ObservableCollection<PinViewModel> PinObs
        {
            get { return _pinList; }
            set { SetProperty(ref _pinList, value); }
        }

        private PinViewModel _SelectedItem;
        public PinViewModel SelectedItem
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


        #endregion

        #region --Private helpers--

        private void OnItemTappedCommand(object p)
        {
            IsListViewVisible = false;
            IsPinTapped = false;
        }

        private void OnMoveToCommand(object position)
        {
            Position newPosition = new Position(SelectedItem.Latitude, SelectedItem.Longitude);
            Random zoom = new Random();

            MoveTo = new MapSpan(newPosition, 3, 3).WithZoom(10 + zoom.NextDouble());
        }



        private void OnTextChangedCommand(object sender)
        {
            if (string.IsNullOrWhiteSpace(SearchBarText))
            {
                PinObs = _Current;
            }
            else
            {
                string low = SearchBarText.ToLower();

                PinObs = new ObservableCollection<PinViewModel>(_Current.Where(pin => (pin.Label.ToLower()).Contains(low) ||
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

            int i = PinList.IndexOf(SelectedPin);

            IsPinTapped = true;
            PinLabel = _Current[i].Label;
            PinDescription = _Current[i].Description;
            PinLatitude = _Current[i].Latitude;
            PinLongitude = _Current[i].Longitude;
        }

        #endregion

        #region --Overrides--

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            PinObs = new ObservableCollection<PinViewModel>((await _pinService.GetUserPinsAsync()).ToOpsOfPinView());
            _Current = PinObs;
            PinList = PinObs.ToListOfPin();
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue<ObservableCollection<PinViewModel>>(nameof(PinObs), out var newPinsValue))
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

            if (parameters.TryGetValue<PinViewModel>(nameof(SelectedItem), out var newSelectedItem))
            {
                SelectedItem = newSelectedItem;
            }

            bool IsUpdated = false;

            if (parameters.TryGetValue<bool>(nameof(IsUpdated), out var newUpdate))
            {
                if (newUpdate)
                {
                    PinList = (await _pinService.GetUserPinsAsync()).ToPinList();
                    PinObs = new ObservableCollection<PinViewModel>((await _pinService.GetUserPinsAsync()).ToOpsOfPinView());
                    _Current = PinObs;
                }
            }


        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedItem) )
            {
                MoveTo = new MapSpan(new Position(SelectedItem.Latitude, SelectedItem.Longitude), 1, 1).WithZoom(10);
                IsListViewVisible = false;
                IsSelected = SelectedItem != null;
                
            }
        }

        #endregion
    }
}
