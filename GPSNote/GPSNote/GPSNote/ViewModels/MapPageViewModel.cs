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
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using GPSNote.Extensions;
using System.ComponentModel;
using GPSNote.Views;
using GPSNote.Servcies.Weather;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.PinShare;
using GPSNote.Servcies.Permission;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace GPSNote.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;
        private readonly IPinService _pinService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPermissionService _permissionService;

        public MapPageViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IPageDialogService pageDialogService, IPinService pinService, IWeatherService weatherService,
            IAuthorizationService authorizationService, IPinShareService pinShareService, IPermissionService permissionService)
            : base(navigationService, localizationService)
        {
            _pageDialogService = pageDialogService;
            _pinService = pinService;
            _authorizationService = authorizationService;
            _permissionService = permissionService;
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


        private MapSpan _MoveTo;
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




        private DelegateCommand<object> _PinClickedCommand;
        public DelegateCommand<object> PinClickedCommand =>
            _PinClickedCommand ?? (_PinClickedCommand = new DelegateCommand<object>(OnPinClicked));


        private DelegateCommand<object> _ItemTappedCommand;
        public DelegateCommand<object> ItemTappedCommand =>
            _ItemTappedCommand ?? (_ItemTappedCommand = new DelegateCommand<object>(OnItemTappedCommand));


        private DelegateCommand<object> _MoveToCommand;
        public DelegateCommand<object> MoveToCommand =>
            _MoveToCommand ?? (_MoveToCommand = new DelegateCommand<object>(OnMoveToCommand));


        private DelegateCommand<object> _SettingsNavigation;
        public DelegateCommand<object> SettingsNavigation =>
            _SettingsNavigation ?? (_SettingsNavigation = new DelegateCommand<object>(OnSettingsNavigation));

        private DelegateCommand<object> _LogOutCommand;
        public DelegateCommand<object> LogOutCommand =>
            _LogOutCommand ?? (_LogOutCommand = new DelegateCommand<object>(OnLogOutCommand));

        #endregion

        #region --Overrides--

        public override async void Initialize(INavigationParameters parameters)
        {
            PermissionStatus permissionStatus = await _permissionService.CheckPermissionsAsync(new LocationPermission());
            
            if ( permissionStatus == PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
            {
                await _permissionService.RequestPermissionAsync(new LocationPermission());
            }
            else if(permissionStatus != PermissionStatus.Granted)
            {
                await _permissionService.RequestPermissionAsync(new LocationPermission());
            }


            base.Initialize(parameters);


            PinObs = new ObservableCollection<PinViewModel>((await _pinService.GetUserPinsAsync()).ToPinViewObservableCollection());
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
                    PinObs = new ObservableCollection<PinViewModel>((await _pinService.GetUserPinsAsync()).ToPinViewObservableCollection());
                    ControlObs = PinObs;
                }
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedListItem))
            {
                MoveTo = new MapSpan(new Position(SelectedListItem.Latitude, SelectedListItem.Longitude), 1, 1).WithZoom(10);
                IsListViewVisible = false;
                IsSelected = SelectedListItem != null;

                DisplayInfoPinViewModel(ControlObs.Where(pinView => pinView.Label.Contains(SelectedListItem.Label)).First());

            }

            if (args.PropertyName == nameof(IsListViewVisible))
            {
                ChangeHeight();
            }

            if (args.PropertyName == nameof(SearchBarText))
            {

                OnTextChangedCommand();
            }

        }

        #endregion

        #region --Private helpers--


        private async void OnSettingsNavigation(object sender)
        {
            await NavigationService.NavigateAsync(nameof(Settings));
        }

        private async void OnLogOutCommand(object sender)
        {
            _authorizationService.LogOut();
            await NavigationService.NavigateAsync($"/{nameof(MainPage)}");
        }

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



        private void OnTextChangedCommand()
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

            NavigationParameters parameter = new NavigationParameters
                {
                    {nameof(PinViewModel), pinView }
                };

            await NavigationService.NavigateAsync(nameof(PinInfoPopup), parameter, true, true);


        }

        #endregion
    }
}