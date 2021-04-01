using GPSNote.Constants;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Views;
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

        #region _____Services____

        private readonly IPageDialogService _pageDialogService;
        private readonly IAuthorizationService _authorization;

        #endregion

        #region ________Private______


        private string _Email;
        private string _Password;
        private bool _IsEnabled;


        private DelegateCommand _NavigateMainListCommand;
        private DelegateCommand _NavigateSignUpCommand;

        #endregion


        public SignInViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IPageDialogService pageDialogService, IAuthorizationService authorization)
            : base(navigationService, localizationService)
        {

            _pageDialogService = pageDialogService;
            _authorization = authorization;


        }


        #region -----Public Properties-----

        public string Email
        {
            get { return _Email; }
            set { SetProperty(ref _Email, value); }
        }

        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }


        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { SetProperty(ref _IsEnabled, value); }
        }

        #endregion


        #region _______Comands______

        public DelegateCommand NavigateMainListButtonTapCommand =>
            _NavigateMainListCommand ??
            (_NavigateMainListCommand = new DelegateCommand(ExecuteNavigateMainViewCommand).ObservesCanExecute(() => IsEnabled));


        public DelegateCommand NavigateSignUpButtonTapCommand =>
            _NavigateSignUpCommand ??
            (_NavigateSignUpCommand = new DelegateCommand(ExecuteNavigateSignUpCommand));

        #endregion 




        #region ________Private Helpers_______

        private async void ExecuteNavigateSignUpCommand()
        {
            await NavigationService.NavigateAsync(nameof(SignUp));

        }

        private async void ExecuteNavigateMainViewCommand()
        {
            if (await _authorization.AuthorizeAsync(Email, Password))
            {
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(TabbedPage1)}");
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync(
                        Resources["Error"], Resources["IncorrectLogPas"], Resources["Ok"]);
            }
        }

        #endregion


        #region ________Overrides_______



        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetValue<string>(Constant.Email) != null)
            {
                Email = parameters.GetValue<string>(Constant.Email);
            }
            Password = string.Empty;

        }




        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(Email) || args.PropertyName == nameof(Password))
            {
                if (Email == null || Password == null) return;

                if (Email != default && Password != default) IsEnabled = true;

                else if (Email != default || Password == default) IsEnabled = false;
            }
        }



        #endregion

    }
}
