using GPSNote.Models;
using GPSNote.Servcies.Authentication;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Validators;
using GPSNote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;
        private readonly IAuthenticationService _AuthenticationService;

        public SignUpViewModel(INavigationService navigationService, ILocalizationService localizationService, IPageDialogService pageDialogService,
            IAuthenticationService authenticationService)
            : base(navigationService, localizationService)
        {

            _pageDialogService = pageDialogService;
            _AuthenticationService = authenticationService;
        }

        #region -----Public Properties-----

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }


        private string _confirmpassword;
        public string ConfirmPassword
        {
            get { return _confirmpassword; }
            set { SetProperty(ref _confirmpassword, value); }
        }

        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { SetProperty(ref _IsEnabled, value); }
        }

        private DelegateCommand _AddUserButtonTapCommand;
        public DelegateCommand AddUserButtonTapCommand =>
            _AddUserButtonTapCommand ?? (_AddUserButtonTapCommand =
            new DelegateCommand(ExecuteUserButtonTapCommand)).ObservesCanExecute(() => IsEnabled);

        #endregion

        #region -----Private Helpers-----

        private async Task<bool> LogPassCheckAsync(string name, string email, string password, string confirmpassword)
        {
            bool result = true;

            if (!Validator.InRange(name, Constant.MinNameLength, Constant.MaxLoginLength))
            {
                await _pageDialogService.DisplayAlertAsync(
                        Resources["Error"], $"{Resources["LogVal1"]} {Constant.MinNameLength} " +
                        $"{Resources["LogVal2"]} {Constant.MaxLoginLength} {Resources["LogVal3"]}", Resources["Ok"]);

                result = false;
            }

            if (result)
            {
                if (!Validator.InRange(password, Constant.MinPasswordLength, Constant.MaxPasswordLength))
                {
                    await _pageDialogService.DisplayAlertAsync(
                            Resources["Error"], $"{Resources["PasVal1"]} {Constant.MinPasswordLength} " +
                            $"{Resources["LogVal2"]} {Constant.MaxPasswordLength} {Resources["LogVal3"]}", Resources["Ok"]);

                    result = false;
                }
            }

            if (result)
            {
                if (Validator.StartWithNumeral(name))
                {
                    await _pageDialogService.DisplayAlertAsync(
                            Resources["Error"], Resources["StartNum"], Resources["Ok"]);

                    result = false;
                }
            }

            if (result)
            {
                if (!Validator.HasUpLowNum(password))
                {
                    await _pageDialogService.DisplayAlertAsync(
                            Resources["Error"], Resources["UpLowNum"], Resources["Ok"]);

                    result = false;
                }
            }

            if (result)
            {
                if (!Validator.IsEmail(email))
                {
                    await _pageDialogService.DisplayAlertAsync(
                            Resources["Error"], Resources["EmailError"], Resources["Ok"]);

                    result = false;
                }
            }

            if (result)
            {
                if (!Validator.Match(password, confirmpassword))
                {
                    await _pageDialogService.DisplayAlertAsync(
                            Resources["Error"], Resources["PasMis"], Resources["Ok"]);

                    result = false;
                }
            }

            return result;
        }

        private async void ExecuteUserButtonTapCommand()
        {
            if (await LogPassCheckAsync(Name, Email, Password, ConfirmPassword))
            {
                if (await _AuthenticationService.SignUpAsync(Name, Email, Password))
                {
                    var p = new NavigationParameters { { Constant.Email, Email } };

                    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignIn)}", p);

                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(Resources["Error"], Resources["EmailExist"], Resources["Ok"]);
                }
            }
        }

        #endregion


        #region -----Overrides-----
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(Name) || args.PropertyName == nameof(Email) || args.PropertyName == nameof(Password) || args.PropertyName == nameof(ConfirmPassword))
            {
                bool result = false;

                if (Name == null || Email == null || Password == null || ConfirmPassword == null)
                {
                    result = true;
                }

                if (result)
                {
                    if (Name != "" && Email != "" && Password != "" && ConfirmPassword != "")
                    {
                        IsEnabled = true;
                    }

                    else if (Name == "" || Email == "" || Password == "" || ConfirmPassword == "")
                    {
                        IsEnabled = false;
                    }
                }     
            }
        }

        #endregion

    }
}
