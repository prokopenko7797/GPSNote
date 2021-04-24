using GPSNote.Servcies.Authentication;
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
        private readonly IAuthenticationService _AuthenticationService;
        private readonly IPermissionService _permissionService;

        public SignInViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IAuthenticationService authentication, IPermissionService permissionService)
            : base(navigationService, localizationService)
        {
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

        private string _EntryBorderColor;
        public string EntryBorderColor
        {
            get { return _EntryBorderColor; }
            set { SetProperty(ref _EntryBorderColor, value); }
        }

        private string _emailerror;
        public string EmailError
        {
            get { return _emailerror; }
            set { SetProperty(ref _emailerror, value); }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }

        private string _passwordError;
        public string PasswordError
        {
            get { return _passwordError; }
            set { SetProperty(ref _passwordError, value); }
        }



        private DelegateCommand _NavigateMainListCommand;
        public DelegateCommand NavigateMainListButtonTapCommand =>
            _NavigateMainListCommand ??
            (_NavigateMainListCommand = new DelegateCommand(ExecuteNavigateMainViewCommand));

        private DelegateCommand _NavigateSignUpCommand;
        public DelegateCommand NavigateSignUpButtonTapCommand =>
            _NavigateSignUpCommand ??
            (_NavigateSignUpCommand = new DelegateCommand(ExecuteNavigateSignUpCommand));

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




        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            _permissionService.CheckPermissionsAsync(new LocationPermission());
        }


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
                if (await _AuthenticationService.CheckUserExist(Email))
                {
                    EmailError = Resources["WrongEmail"];
                }
                PasswordError = Resources["WrongPass"];
            }
        }

        #endregion

    }
}
