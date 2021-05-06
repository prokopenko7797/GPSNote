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

            EmailBorderColor = (Color)App.Current.Resources["System/LightGray"];
            NameBorderColor = (Color)App.Current.Resources["System/LightGray"];
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

        private Color _EntryBorderColor;
        public Color EmailBorderColor
        {
            get { return _EntryBorderColor; }
            set { SetProperty(ref _EntryBorderColor, value); }
        }

        private Color _NameBorderColor;
        public Color NameBorderColor
        {
            get { return _NameBorderColor; }
            set { SetProperty(ref _NameBorderColor, value); }
        }

        private DelegateCommand _AddUserButtonTapCommand;
        public DelegateCommand AddUserButtonTapCommand =>
            _AddUserButtonTapCommand ?? (_AddUserButtonTapCommand =
            new DelegateCommand(ExecuteUserButtonTapCommand).ObservesCanExecute(() => IsEnabled));

        private DelegateCommand _BackButtonCommand;
        public DelegateCommand BackButtonCommand =>
            _BackButtonCommand ??
            (_BackButtonCommand = new DelegateCommand(OnBackButtonCommand));

        #endregion

        #region -- Overrides --

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


        private async void OnBackButtonCommand()
        {
            await NavigationService.GoBackAsync();
        }


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

                if (!Validator.CheckInRange(Name, Constant.MinNameLength, Constant.MaxLoginLength))
                {
                    if (Application.Current.UserAppTheme == OSAppTheme.Light)
                    {
                        NameBorderColor = (Color)App.Current.Resources["Light/Error"];
                    }
                    else
                    {
                        NameBorderColor = (Color)App.Current.Resources["Dark/Error"];
                    }

                    NameError = Resources["NickNameError"];
                }
                else
                {
                    NameError = string.Empty;
                    NameBorderColor = (Color)App.Current.Resources["System/LightGray"];

                    if (!await _AuthenticationService.CheckUserExistAsync(Email))
                    {

                        NameError = string.Empty;
                        NameBorderColor = (Color)App.Current.Resources["System/LightGray"];
                        EmailError = string.Empty;
                        EmailBorderColor = (Color)App.Current.Resources["System/LightGray"];

                        var p = new NavigationParameters();
                        p.Add(nameof(Name), Name);
                        p.Add(nameof(Email), Email);


                        await NavigationService.NavigateAsync(nameof(CreateAccount), p);
                    }
                    else
                    {
                        if (Application.Current.UserAppTheme == OSAppTheme.Light)
                        {
                            EmailBorderColor = (Color)App.Current.Resources["Light/Error"];
                        }
                        else
                        {
                            EmailBorderColor = (Color)App.Current.Resources["Dark/Error"];
                        }

                        EmailError = Resources["EmailExist"];
                    }

                }

                
            }
        }

        #endregion
    }
}
