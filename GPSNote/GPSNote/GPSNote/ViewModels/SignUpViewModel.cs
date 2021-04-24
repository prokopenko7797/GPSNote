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

        private string _errorname;
        public string NameError
        {
            get { return _errorname; }
            set { SetProperty(ref _errorname, value); }
        }

        private string _emailerror;
        public string EmailError
        {
            get { return _emailerror; }
            set { SetProperty(ref _emailerror, value); }
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

        #region -----Private Helpers-----

        private bool LoginCheck(string name, string email)
        {
            bool result = true;

            if (!Validator.CheckInRange(name, Constant.MinNameLength, Constant.MaxLoginLength))
            {
                NameError = Resources["InRange"];

                result = false;
            }

            if (Validator.CheckStartWithNumeral(name))
            {

                NameError = Resources["StartNum"];
                result = false;
            }



            if (!Validator.IsEmail(email))
            {
                EmailError = Resources["EmailError"];

                result = false;
            }


            return result;
        }

        private async void ExecuteUserButtonTapCommand()
        {
            if (LoginCheck(Name, Email))
            {
                if (await _AuthenticationService.CheckUserExistAsync(Email))
                {
                    var p = new NavigationParameters();
                    p.Add(nameof(Name), Name);
                    p.Add(nameof(Email), Email);

                    await NavigationService.NavigateAsync(nameof(CreateAccount), p);
                }
                else
                {
                    EmailError = Resources["EmailExist"];
                }
            }
        }

        #endregion
    }
}
