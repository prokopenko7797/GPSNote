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

namespace GPSNote.ViewModels
{
    public class PinListViewModel : ViewModelBase
    {
        
        private ObservableCollection<UserPins> _pinList;

        private ICommand _LogOutToolBarCommand;
        private ICommand _SettingsToolBarCommand;
        private ICommand _AddEditButtonClicked;
        private ICommand _DeleteCommandTap;
        private ICommand _EditCommandTap;

        private bool _IsVisible;

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
        public ObservableCollection<UserPins> PinList
        {
            get { return _pinList; }
            set { SetProperty(ref _pinList, value); }
        }

        public bool IsVisible
        {
            get { return _IsVisible; }
            set { SetProperty(ref _IsVisible, value); }
        }

        public ICommand LogOutToolBarCommand =>
            _LogOutToolBarCommand ?? (_LogOutToolBarCommand =
            new Command(NavigateLogOutToolBarCommand));

        public ICommand SettingsToolBarCommand =>
            _SettingsToolBarCommand ?? (_SettingsToolBarCommand =
            new Command(NavigateSettingsCommand));

        public ICommand AddEditButtonClicked =>
            _AddEditButtonClicked ?? (_AddEditButtonClicked =
            new Command(NavigateAddEditProfileCommand));

        public ICommand EditCommandTap => _EditCommandTap ?? (_EditCommandTap = new Command(EditCommand));

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

           // await NavigationService.NavigateAsync(nameof(AddEditProfile), parametrs);
        }


     
        private async void NavigateAddEditProfileCommand()
        {
           // await NavigationService.NavigateAsync(nameof(AddEditProfile));

        }


        private async void NavigateSettingsCommand()
        {
            await NavigationService.NavigateAsync(nameof(Settings));
        }


        private async void NavigateLogOutToolBarCommand()
        {
            _authorizationService.LogOut();
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignIn)}");
        }

        #endregion

        #region --Overrides--

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            UpdateCollection();


        }

        private async void UpdateCollection()
        {
            PinList = new ObservableCollection<UserPins>(await _pinService.GetUserPinsAsync());

            IsVisible = PinList.Count() == 0;
        }

        #endregion
    }
}

