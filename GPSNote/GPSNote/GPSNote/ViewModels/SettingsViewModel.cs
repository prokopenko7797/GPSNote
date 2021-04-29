using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.Settings;
using GPSNote.Servcies.ThemeService;
using GPSNote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IThemeService _ThemeService;
        private OSAppTheme _appTheme;

        public SettingsViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IThemeService themeService)
            : base(navigationService, localizationService)
        {
            _ThemeService = themeService;
        }

        #region -- Public properties --


        private bool _IsChecked;

        public bool IsChecked
        {
            get { return _IsChecked; }
            set { SetProperty(ref _IsChecked, value); }
        }

       

        private DelegateCommand _BackButtonCommand;
        public DelegateCommand BackButtonCommand =>
            _BackButtonCommand ??
            (_BackButtonCommand = new DelegateCommand(OnBackButtonCommand));


        private DelegateCommand _ToLangSettings;
        public DelegateCommand ToLangSettings =>
            _ToLangSettings ??
            (_ToLangSettings = new DelegateCommand(OnToLangSettingsCommand));

        #endregion

        #region --Overrides--




        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            _appTheme = _ThemeService.GetCurrentTheme();

            if (_appTheme == OSAppTheme.Light)
            {
                IsChecked = false;
            }
            else 
            {
                IsChecked = true;
            }
        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsChecked))
            {
                if (IsChecked == true)
                {
                    _ThemeService.SetTheme(OSAppTheme.Dark);

                }
                else
                {
                    _ThemeService.SetTheme(OSAppTheme.Light);
                }

            }
        }


        #endregion

        #region -- Private helpers --


        
        private async void OnToLangSettingsCommand()
        {
            await NavigationService.NavigateAsync(nameof(LangSettings));
        }


        private async void OnBackButtonCommand()
        {
            await NavigationService.GoBackAsync();
        }


        #endregion

    }
}
