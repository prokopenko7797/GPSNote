using GPSNote.Constants;
using GPSNote.Servcies.LocalizationService;
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

        private string _name;
        private string _email;
        private string _password;
        private string _confirmpassword;



        private DelegateCommand _AddUserButtonTapCommand;

        #endregion


        public SignUpViewModel(INavigationService navigationService, ILocalizationService localizationService, IPageDialogService pageDialogService,
            IRegistrationService registrationService)
            : base(navigationService, localizationService)
        {

            _pageDialogService = pageDialogService;
            _registrationService = registrationService;

        }




        #region -----Public Properties-----


        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
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

        private async Task<bool> LogPassCheck(string name, string email, string password, string confirmpassword)
        {
            if (!Validator.InRange(name, Constant.MinLoginLength, Constant.MaxLoginLength))
            {
                await _pageDialogService.DisplayAlertAsync(
                        Resources["Error"], $"{Resources["LogVal1"]} {Constant.MinLoginLength} " +
                        $"{Resources["LogVal2"]} {Constant.MaxLoginLength} {Resources["LogVal3"]}", Resources["Ok"]);
                return false;
            }

            if (!Validator.InRange(password, Constant.MinPasswordLength, Constant.MaxPasswordLength))
            {
                await _pageDialogService.DisplayAlertAsync(
                        Resources["Error"], $"{Resources["PasVal1"]} {Constant.MinPasswordLength} " +
                        $"{Resources["LogVal2"]} {Constant.MaxPasswordLength} {Resources["LogVal3"]}", Resources["Ok"]);
                return false;
            }


            if (Validator.StartWithNumeral(name))
            {
                await _pageDialogService.DisplayAlertAsync(
                        Resources["Error"], Resources["StartNum"], Resources["Ok"]);
                return false;
            }

            if (!Validator.HasUpLowNum(password))
            {
                await _pageDialogService.DisplayAlertAsync(
                        Resources["Error"], Resources["UpLowNum"], Resources["Ok"]);
                return false;
            }

            if (!Validator.IsEmail(email))
            {

                await _pageDialogService.DisplayAlertAsync(
                        Resources["Error"], Resources["EmailError"], Resources["Ok"]);
                return false;
            }

            if (!Validator.Match(password, confirmpassword))
            {

                await _pageDialogService.DisplayAlertAsync(
                        Resources["Error"], Resources["PasMis"], Resources["Ok"]);
                return false;
            }
            return true;

        }


        private async void ExecuteddUserButtonTapCommand()
        {
            if (await LogPassCheck(Name, Email, Password, ConfirmPassword))
            {
                if (await _registrationService.RegistrateAsync(Name, Email, Password))
                {
                    var p = new NavigationParameters { { Constant.Email, Email } };

                    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignIn)}", p);

                }
                else await _pageDialogService.DisplayAlertAsync(Resources["Error"], Resources["EmailExist"], Resources["Ok"]);
            }


        }



        #endregion


        #region -----Overrides-----
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(Name) || args.PropertyName == nameof(Email) || args.PropertyName == nameof(Password) || args.PropertyName == nameof(ConfirmPassword))
            {
                if (Name == null || Email == null || Password == null || ConfirmPassword == null) return;

                if (Name != "" && Email != "" && Password != "" && ConfirmPassword != "") IsEnabled = true;

                else if (Name == "" || Email == "" || Password == "" || ConfirmPassword == "") IsEnabled = false;
            }
        }

        #endregion

    }
}
