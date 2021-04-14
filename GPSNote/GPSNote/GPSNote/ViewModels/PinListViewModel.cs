using Acr.UserDialogs;
using GPSNote.Models;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
using GPSNote.Views;
using GPSNote.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.ComponentModel;
using GPSNote.ViewModels.ExtendedViewModels;
using System.Threading.Tasks;
using Prism.Common;

namespace GPSNote.ViewModels
{
   

    public class PinListViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IAuthorizationService _authorizationService;

        private ObservableCollection<PinViewModel> _ControlObs; 

        public PinListViewModel(INavigationService navigationService, ILocalizationService localizationService, IPinService pinService,
            IAuthorizationService authorizationService)
            : base(navigationService, localizationService)
        {
            _authorizationService = authorizationService;
            _pinService = pinService;
        }

        #region -- Public properties --
        private ObservableCollection<PinViewModel> _pinList;
        public ObservableCollection<PinViewModel> PinObs
        {
            get { return _pinList; }
            set { SetProperty(ref _pinList, value); }
        }

        private bool _IsVisible;
        public bool IsVisible
        {
            get { return _IsVisible; }
            set { SetProperty(ref _IsVisible, value); }
        }


        private PinViewModel _SelectedItem;
        public PinViewModel SelectedItem
        {
            get { return _SelectedItem; }
            set { SetProperty(ref _SelectedItem, value); }
        }


        string _SearchBarText;
        public string SearchBarText
        {
            get { return _SearchBarText; }
            set { SetProperty(ref _SearchBarText, value); }
        }

        private DelegateCommand<object> _LogOutToolBarCommand;
        public DelegateCommand<object> LogOutToolBarCommand =>
            _LogOutToolBarCommand ?? (_LogOutToolBarCommand = new DelegateCommand<object>(OnLogOutToolBarCommand));
       
        private DelegateCommand<object> _SettingsToolBarCommand;
        public DelegateCommand<object> SettingsToolBarCommand =>
            _SettingsToolBarCommand ?? (_SettingsToolBarCommand = new DelegateCommand<object>(OnSettingsCommand));

        private DelegateCommand<object> _AddEditButtonClicked;
        public DelegateCommand<object> AddEditButtonClicked =>
            _AddEditButtonClicked ?? (_AddEditButtonClicked = new DelegateCommand<object>(OnNavigateAddEditPinCommand));

        private DelegateCommand<object> _EditCommandTap;
        public DelegateCommand<object> EditCommandTap => 
            _EditCommandTap ?? (_EditCommandTap = new DelegateCommand<object>(OnEditCommand));

        private DelegateCommand<object> _DeleteCommandTap;
        public DelegateCommand<object> DeleteCommandTap => 
            _DeleteCommandTap ?? (_DeleteCommandTap = new DelegateCommand<object>(OnDeleteCommand));

        private DelegateCommand<object> _ImageCommandTap;
        public DelegateCommand<object> ImageCommandTap => 
            _ImageCommandTap ?? (_ImageCommandTap = new DelegateCommand<object>(OnChangeVisibilityComand));

        private DelegateCommand<object> _OnTextChangedCommand;
        public DelegateCommand<object> OnTextChangedCommand =>
            _OnTextChangedCommand ?? (_OnTextChangedCommand = new DelegateCommand<object>(OnSearchCommand));

        private DelegateCommand<object> _ItemTappedCommand;
        public DelegateCommand<object> ItemTappedCommand =>
            _ItemTappedCommand ?? (_ItemTappedCommand = new DelegateCommand<object>(OnItemTappedAsyncCommand));

        #endregion



        #region -- Private helpers --

        private async void OnItemTappedAsyncCommand(object sender)
        {
            var parameters = new NavigationParameters
                {
                    { nameof(SelectedItem), SelectedItem }
                };

            await NavigationService.SelectTabAsync($"{nameof(MapPage)}", parameters);
        }


        private void OnSearchCommand(object sender)
        {
            if (string.IsNullOrWhiteSpace(SearchBarText))
            {
                PinObs = _ControlObs;
            }
            else
            {
                string low = SearchBarText.ToLower();

                PinObs = new ObservableCollection<PinViewModel>(_ControlObs.Where(pin =>(pin.Label.ToLower()).Contains(low) ||
                                                                                  (pin.Description.ToLower()).Contains(low) ||
                                                                                  (pin.Latitude.ToString()).Contains(low) ||
                                                                                  (pin.Longitude.ToString()).Contains(low)));
            }
        }

        private async void OnDeleteCommand(object sender)
        {
            if (!(sender is PinViewModel userPinsV)) return;

            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Message = Resources["Delete?"],
                OkText = Resources["Yes"],
                CancelText = Resources["No"]
            });
            if (result)
            {
                await _pinService.DeletePinAsync(userPinsV.Id);
                PinObs.Remove(userPinsV);
                _ControlObs.Remove(userPinsV);
                if (PinObs.Count() == 0) IsVisible = true;
            }
        }

        private async void OnEditCommand(object sender)
        {
            PinViewModel pins = sender as PinViewModel;
            
            var parametrs = new NavigationParameters
            {
                { nameof(PinViewModel), pins }
            };
            
            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(AddEditPin)}", parametrs);
        }

        private async void OnChangeVisibilityComand(object sender)
        {
            PinViewModel pin = sender as PinViewModel;

            int i = PinObs.IndexOf(pin);
            int j = _ControlObs.IndexOf(pin);
            pin.IsEnabled = !pin.IsEnabled;
            PinObs[i] = pin;
            _ControlObs[j] = pin;

            await _pinService.EditPinAsync(pin.ToPinModel());           
        }

        private async void OnNavigateAddEditPinCommand(object sender)
        {
            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(AddEditPin)}");
        }

        private async void OnSettingsCommand(object sender)
        {
            await NavigationService.NavigateAsync(nameof(Settings));
        }

        private async void OnLogOutToolBarCommand(object sender)
        {
            _authorizationService.LogOut();
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignIn)}");
        }

        private async void UpdateCollectionAsync()
        {
            PinObs = new ObservableCollection<PinViewModel>((await _pinService.GetUserPinsAsync()).ToOpsOfPinView());
            _ControlObs = PinObs;
            IsVisible = PinObs.Count() == 0;
        }

        #endregion

        #region --Overrides--

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            bool IsUpdated = false;

            if (parameters.TryGetValue<bool>(nameof(IsUpdated), out var newUpdate))
            {
                if (newUpdate)
                {
                    UpdateCollectionAsync();
                }
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            UpdateCollectionAsync();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            parameters.Add(nameof(PinViewModel), _ControlObs);
        }

        #endregion
    }
}

