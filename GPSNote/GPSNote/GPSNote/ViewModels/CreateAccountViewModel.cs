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
    public class CreateAccountViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _AuthenticationService;

        private string _name;
        private string _email;


        public CreateAccountViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IAuthenticationService authenticationService)
            : base(navigationService, localizationService)
        {
            _AuthenticationService = authenticationService;
        }

        #region -----Public Properties-----

        private string _passwordError;
        public string PasswordError
        {
            get { return _passwordError; }
            set { SetProperty(ref _passwordError, value); }
        }


        private string _confPassError;
        public string ConfPassError
        {
            get { return _confPassError; }
            set { SetProperty(ref _confPassError, value); }
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

        private bool PassCheck(string password, string confirmpassword)
        {
            bool result = true;


            if (!Validator.InRange(password, Constant.MinPasswordLength, Constant.MaxPasswordLength))
            {
                PasswordError = Resources["IncorrectPass"];

                result = false;
            }


            if (result)
            {
                if (!Validator.HasUpLowNum(password))
                {
                    PasswordError = Resources["IncorrectPass"];

                    result = false;
                }
            }

            if (!Validator.Match(password, confirmpassword))
            {
                ConfPassError = Resources["PasMis"];

                result = false;
            }


            return result;
        }

        private async void ExecuteUserButtonTapCommand()
        {
            if (PassCheck(Password, ConfirmPassword))
            {
                if (await _AuthenticationService.SignUpAsync(_name, _email, Password))
                {
                    var p = new NavigationParameters { { Constant.Email, _email } };

                    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignIn)}", p);

                }
                else
                {
                    PasswordError = Resources["EmailExist"];
                }
            }
        }

        #endregion


        #region -----Overrides-----
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(Password) || args.PropertyName == nameof(ConfirmPassword))
            {
                bool result = false;

                if (Password == null || ConfirmPassword == null)
                {
                    result = true;
                }

                if (result)
                {
                    if (Password != string.Empty && ConfirmPassword != string.Empty)
                    {
                        IsEnabled = true;
                    }

                    else if (Password == string.Empty || ConfirmPassword == string.Empty)
                    {
                        IsEnabled = false;
                    }
                }
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<string>(Constant.Email, out string email))
            {
                _email = email;
            }

            if (parameters.TryGetValue<string>(Constant.Name, out string name))
            {
                _name = name;
            }
        }

        #endregion
    }
}
