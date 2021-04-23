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
        private readonly IThemeService _ThemeService;
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
           (_SaveToolBarCommand = new DelegateCommand(OnSaveToolBar));

        #endregion

        #region --Overrides--

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            SelectedLang = Resources.Lang;

            switch (Resources.Lang)
            {
                case Constant.ResourcesLangConst.En:
                    IsCheckedEn = true;
                    break;

                case Constant.ResourcesLangConst.Ru:
                    IsCheckedRu = true;
                    break;
            }

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

            if (args.PropertyName == nameof(SelectedLang))
            {
                Resources.ChangeCulture(SelectedLang);
                Resources.Lang = SelectedLang;
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            _ThemeService.SetTheme(_appTheme);
            Resources.ChangeCulture(Resources.Lang);
        }

        #endregion

        #region -- Private helpers --

        private async void OnSaveToolBar()
        {
            _appTheme = _ThemeService.GetCurrentTheme();
            Resources.Lang = SelectedLang;

            await NavigationService.GoBackAsync();
        }

        #endregion

    }
}
