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

        private async Task<bool> LoginCheckAsync(string name, string email)
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
                if (Validator.StartWithNumeral(name))
                {
                    await _pageDialogService.DisplayAlertAsync(
                            Resources["Error"], Resources["StartNum"], Resources["Ok"]);

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

            return result;
        }

        private async void ExecuteUserButtonTapCommand()
        {
            if (await LoginCheckAsync(Name, Email))
            {
                if (await _AuthenticationService.CheckUserExist(Email))
                {
                    var p = new NavigationParameters();
                    p.Add(nameof(Name), Name);
                    p.Add(nameof(Email), Email);

                    await NavigationService.NavigateAsync(nameof(CreateAccount), p);
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
            if (args.PropertyName == nameof(Name) || args.PropertyName == nameof(Email))
            {
                bool result = false;

                if (Name == null || Email == null)
                {
                    result = true;
                }

                if (result)
                {
                    if (Name != string.Empty && Email != string.Empty)
                    {
                        IsEnabled = true;
                    }

                    else if (Name == string.Empty || Email == string.Empty)
                    {
                        IsEnabled = false;
                    }
                }     
            }
        }

        #endregion

    }
}
