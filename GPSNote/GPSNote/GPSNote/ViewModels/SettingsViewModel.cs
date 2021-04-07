using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.Settings;
using GPSNote.Servcies.ThemeService;
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
        private IThemeService _ThemeService;
        private OSAppTheme _appTheme;

        public SettingsViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IThemeService themeService)
            : base(navigationService, localizationService)
        {
            _ThemeService = themeService;
        }

        #region -- Public properties --

        private string _SelectedLang;
        
        public string SelectedLang
        {
            get { return _SelectedLang; }
            set { SetProperty(ref _SelectedLang, value); }
        }

        private bool _IsChecked;

        public bool IsChecked
        {
            get { return _IsChecked; }
            set { SetProperty(ref _IsChecked, value); }
        }

        private bool _IsCheckedEn;
        public bool IsCheckedEn
        {
            get { return _IsCheckedEn; }
            set { SetProperty(ref _IsCheckedEn, value); }
        }

        private bool _IsCheckedRu;
        public bool IsCheckedRu
        {
            get { return _IsCheckedRu; }
            set { SetProperty(ref _IsCheckedRu, value); }
        }

        private DelegateCommand _SaveToolBarCommand;
        public DelegateCommand SaveToolBarCommand =>
           _SaveToolBarCommand ??
           (_SaveToolBarCommand = new DelegateCommand(SaveToolBar));

        #endregion

        #region -- Private helpers --

        private async void SaveToolBar()
        {
            _ThemeService.Theme = (int)_appTheme;

            Resources.Lang = SelectedLang;

            await NavigationService.GoBackAsync();
        }

        #endregion

        #region --Overrides--

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            SelectedLang = Resources.Lang;

            switch (Resources.Lang)
            {
                case Constant.ResourcesLangConst.en:
                    IsCheckedEn = true;

                    break;
                case Constant.ResourcesLangConst.ru:
                    IsCheckedRu = true;
                    break;
            }

            _appTheme = (OSAppTheme)_ThemeService.Theme;

            if (_ThemeService.Theme == (int)OSAppTheme.Unspecified)
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
                    _appTheme = OSAppTheme.Dark;
                    Application.Current.UserAppTheme = OSAppTheme.Dark;

                }
                else
                {
                    _appTheme = OSAppTheme.Unspecified;
                    Application.Current.UserAppTheme = OSAppTheme.Unspecified;
                }

                _ThemeService.Theme = (int)_appTheme;
            }

            if (args.PropertyName == nameof(SelectedLang))
            {
                Resources.ChangeCulture(SelectedLang);
                Resources.Lang = SelectedLang;
            }
        }



        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            Application.Current.UserAppTheme = (OSAppTheme)_ThemeService.Theme;
            Resources.ChangeCulture(Resources.Lang);
        }

        #endregion

    }
}
