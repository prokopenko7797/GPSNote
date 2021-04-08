using Acr.UserDialogs;
using GPSNote.Models;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
using GPSNote.Views;
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

namespace GPSNote.ViewModels
{
    public class PinListViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IAuthorizationService _authorizationService;


        public PinListViewModel(INavigationService navigationService, ILocalizationService localizationService, IPinService pinService,
            IAuthorizationService authorizationService)
            : base(navigationService, localizationService)
        {
            _authorizationService = authorizationService;
            _pinService = pinService;
        }

        #region -- Public properties --
        private ObservableCollection<UserPins> _pinList;
        public ObservableCollection<UserPins> PinList
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

        private ICommand _LogOutToolBarCommand;
        public ICommand LogOutToolBarCommand =>
            _LogOutToolBarCommand ?? (_LogOutToolBarCommand =
            new Command(NavigateLogOutToolBarCommand));
       
        private ICommand _SettingsToolBarCommand;
        public ICommand SettingsToolBarCommand =>
            _SettingsToolBarCommand ?? (_SettingsToolBarCommand =
            new Command(NavigateSettingsCommand));

        private ICommand _AddEditButtonClicked;
        public ICommand AddEditButtonClicked =>
            _AddEditButtonClicked ?? (_AddEditButtonClicked =
            new Command(NavigateAddEditProfileCommand));


        private ICommand _EditCommandTap;
        public ICommand EditCommandTap => _EditCommandTap ?? (_EditCommandTap = new Command(EditCommand));

        private ICommand _DeleteCommandTap;
        public ICommand DeleteCommandTap => _DeleteCommandTap ?? (_DeleteCommandTap = new Command(DeleteCommand));

        #endregion



        #region -- Private helpers --


        private async void DeleteCommand(object sender)
        {
            if (!(sender is UserPins userPins)) return;

            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Message = Resources["Delete?"],
                OkText = Resources["Yes"],
                CancelText = Resources["No"]
            });
            if (result)
            {
                await _pinService.DeletePinAsync(userPins.id);
                PinList.Remove(userPins);
                if (PinList.Count() == 0) IsVisible = true;
            }
        }


        private async void EditCommand(object sender)
        {
            UserPins pins = sender as UserPins;
            
            var parametrs = new NavigationParameters
            {
                { nameof(UserPins), pins }
            };

            await NavigationService.NavigateAsync(nameof(Settings), parametrs);
        }


     
        private async void NavigateAddEditProfileCommand()
        {
            await NavigationService.NavigateAsync(nameof(AddEditPin));

        }


        private async void NavigateSettingsCommand()
        {
            await NavigationService.NavigateAsync(nameof(AddEditPin));
        }


        private async void NavigateLogOutToolBarCommand()
        {
            _authorizationService.LogOut();
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignIn)}");
        }

        private async void UpdateCollection()
        {
            PinList = new ObservableCollection<UserPins>(await _pinService.GetUserPinsAsync());

            IsVisible = PinList.Count() == 0;
        }

        #endregion

        #region --Overrides--

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            UpdateCollection();

        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            UpdateCollection();
        }


        #endregion
    }
}

