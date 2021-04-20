﻿using GPSNote.Servcies.Authentication;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.Permission;
using GPSNote.Views;
using Plugin.Permissions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;
        private readonly IAuthenticationService _AuthenticationService;
        private readonly IPermissionService _permissionService;

        public SignInViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IPageDialogService pageDialogService, IAuthenticationService authentication, IPermissionService permissionService)
            : base(navigationService, localizationService)
        {
            _pageDialogService = pageDialogService;
            _AuthenticationService = authentication;
            _permissionService = permissionService;
        }

        #region -----Public Properties-----

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { SetProperty(ref _Email, value); }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }

        private bool _IsEnabledButton;
        public bool IsEnabledButton
        {
            get { return _IsEnabledButton; }
            set { SetProperty(ref _IsEnabledButton, value); }
        }

        private DelegateCommand _NavigateMainListCommand;
        public DelegateCommand NavigateMainListButtonTapCommand =>
            _NavigateMainListCommand ??
            (_NavigateMainListCommand = new DelegateCommand(ExecuteNavigateMainViewCommand).ObservesCanExecute(() => IsEnabledButton));

        private DelegateCommand _NavigateSignUpCommand;
        public DelegateCommand NavigateSignUpButtonTapCommand =>
            _NavigateSignUpCommand ??
            (_NavigateSignUpCommand = new DelegateCommand(ExecuteNavigateSignUpCommand));

        #endregion 
        #region -- Private helpers --

        private async void ExecuteNavigateSignUpCommand()
        {
            await NavigationService.NavigateAsync(nameof(SignUp));

        }

        private async void ExecuteNavigateMainViewCommand()
        {
            if (await _AuthenticationService.SignInAsync(Email, Password))
            {
                await NavigationService.NavigateAsync($"/{nameof(MainTabbedPage)}");
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync(
                        Resources["Error"], Resources["IncorrectLogPas"], Resources["Ok"]);
            }
        }

        #endregion
        #region --Overrides--

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<string>(Constant.Email, out string email))
            {
                Email = email;
            }
            Password = string.Empty;

        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(Email) || args.PropertyName == nameof(Password))
            {
                bool result = false;

                if (Email != null || Password != null)
                {
                    result = true;
                }

                if (result)
                {
                    if (Email != default && Password != default)
                    {
                        IsEnabledButton = true;
                    }

                    else if (Email != default || Password == default)
                    {
                        IsEnabledButton = false;
                    }
                }                
            }
        }


        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            _permissionService.CheckPermissionsAsync(new LocationPermission());
        }


        #endregion

    }
}
