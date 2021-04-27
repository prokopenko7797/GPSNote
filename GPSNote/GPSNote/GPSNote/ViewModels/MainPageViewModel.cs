using GPSNote.Servcies.LocalizationService;
using GPSNote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService, ILocalizationService localizationService)
            : base(navigationService, localizationService)
        {

        }


        #region -----Public Properties-----

        private DelegateCommand _LogInCommand;
        public DelegateCommand LogInCommand =>
            _LogInCommand ??
            (_LogInCommand = new DelegateCommand(OnLogInCommand));

        private DelegateCommand _CreateAccountCommand;
        public DelegateCommand CreateAccountCommand =>
            _CreateAccountCommand ??
            (_CreateAccountCommand = new DelegateCommand(OnCreateAccountCommand));

        #endregion 

        #region -- Private helpers --

        private async void OnCreateAccountCommand()
        {
            await NavigationService.NavigateAsync(nameof(SignUp));

        }

        private async void OnLogInCommand()
        {
            await NavigationService.NavigateAsync(nameof(SignIn));
        }

        #endregion

    }
}
