using GPSNote.Constants;
using GPSNote.Servcies.RegistrationService;
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
        #region _______Services______

        private readonly IPageDialogService _pageDialogService;
        private readonly IRegistrationService _registrationService;

        #endregion

        #region _______Private_______

        private string _login;
        private string _password;
        private string _confirmpassword;



        private DelegateCommand _AddUserButtonTapCommand;

        #endregion


        public SignUpViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IRegistrationService registrationService)
            : base(navigationService)
        {

            _pageDialogService = pageDialogService;
            _registrationService = registrationService;

        }




        #region -----Public Properties-----


        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }


        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }


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

        #endregion


        #region ________Comands________
        public DelegateCommand AddUserButtonTapCommand =>
            _AddUserButtonTapCommand ?? (_AddUserButtonTapCommand =
            new DelegateCommand(ExecuteddUserButtonTapCommand)).ObservesCanExecute(() => IsEnabled);


        #endregion

        #region -----Private Helpers-----

        private async Task<bool> LogPassCheck(string login, string password, string confirmpassword)
        {
            if (!Validator.InRange(login, Constant.MinLoginLength, Constant.MaxLoginLength))
            {
                await _pageDialogService.DisplayAlertAsync(
                        "Error", $"Login must be at least {Constant.MinLoginLength} " +
                        $" and not more then {Constant.MaxLoginLength} symbols.", "Ok");
                return false;
            }

            if (!Validator.InRange(password, Constant.MinPasswordLength, Constant.MaxPasswordLength))
            {
                await _pageDialogService.DisplayAlertAsync(
                        "Error", $"Password must be at least {Constant.MinPasswordLength} " +
                        $"and not more then {Constant.MaxPasswordLength} symbols", "Ok");
                return false;
            }


            if (Validator.StartWithNumeral(login))
            {
                await _pageDialogService.DisplayAlertAsync(
                        "Error", "Login should not start with number.", "Ok");
                return false;
            }

            if (!Validator.HasUpLowNum(password))
            {
                await _pageDialogService.DisplayAlertAsync(
                       "Error", "Password must contain at least one uppercase letter, one lowercase letter and one number.", "Ok");
                return false;
            }

            if (!Validator.Match(password, confirmpassword))
            {

                await _pageDialogService.DisplayAlertAsync(
                        "Error", "Password mismatch.", "Ok");
                return false;
            }
            return true;

        }


        private async void ExecuteddUserButtonTapCommand()
        {
            if (await LogPassCheck(Login, Password, ConfirmPassword))
            {
                if (await _registrationService.RegistrateAsync(Login, Password))
                {
                    var p = new NavigationParameters { { Constant.Login, Login } };

                    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignIn)}", p);

                }
                else await _pageDialogService.DisplayAlertAsync("Error", "Login already exist.", "Ok");
            }


        }



        #endregion


        #region -----Overrides-----


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            {
                base.OnPropertyChanged(args);
                if (args.PropertyName == nameof(Login) || args.PropertyName == nameof(Password) || args.PropertyName == nameof(ConfirmPassword))
                {
                    if (Login == null || Password == null || ConfirmPassword == null) return;

                    if (Login != "" && Password != "" && ConfirmPassword != "") IsEnabled = true;

                    else if (Login == "" || Password == "" || ConfirmPassword == "") IsEnabled = false;
                }
            }
        }



        #endregion

    }
}
