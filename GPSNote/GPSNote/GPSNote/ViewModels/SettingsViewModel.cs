using GPSNote.Constants;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.Settings;
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

        ISettingsManager _settingsManager;


        #region ____Private_____

        private DelegateCommand _SaveToolBarCommand;

        private OSAppTheme appTheme;

        private string _SelectedLang;
        private bool _IsChecked;
        private bool _IsCheckedEn;
        private bool _IsCheckedRu;






        #endregion

        public SettingsViewModel(INavigationService navigationService, ILocalizationService localizationService,
            ISettingsManager settingsManager)
            : base(navigationService, localizationService)
        {
            _settingsManager = settingsManager;
        }



        #region ______Public Properties______


        public string SelectedLang
        {
            get { return _SelectedLang; }
            set { SetProperty(ref _SelectedLang, value); }
        }

        public bool IsChecked
        {
            get { return _IsChecked; }
            set { SetProperty(ref _IsChecked, value); }
        }

        public bool IsCheckedEn
        {
            get { return _IsCheckedEn; }
            set { SetProperty(ref _IsCheckedEn, value); }
        }

        public bool IsCheckedRu
        {
            get { return _IsCheckedRu; }
            set { SetProperty(ref _IsCheckedRu, value); }
        }







        #endregion


        #region _______Comands_______

        public DelegateCommand SaveToolBarCommand =>
           _SaveToolBarCommand ??
           (_SaveToolBarCommand = new DelegateCommand(SaveToolBar));

        #endregion

        #region _____Private Helpers______

        private async void SaveToolBar()
        {
            _settingsManager.Theme = (int)appTheme;

            _settingsManager.Lang = SelectedLang;

            await NavigationService.GoBackAsync();
        }





        #endregion

        #region ________Overrides_________

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            SelectedLang = _settingsManager.Lang;

  

            switch (_settingsManager.Lang)
            {
                case Constant.ResourcesLangConst.en:
                    IsCheckedEn = true;
                    break;
                case Constant.ResourcesLangConst.ru:
                    IsCheckedRu = true;
                    break;
            }

            appTheme = (OSAppTheme)_settingsManager.Theme;

            if (_settingsManager.Theme == (int)OSAppTheme.Unspecified)
                IsChecked = false;
            else IsChecked = true;
        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsChecked))
            {
                if (IsChecked == true)
                {
                    appTheme = OSAppTheme.Dark;
                    Application.Current.UserAppTheme = OSAppTheme.Dark;
                }
                else
                {
                    appTheme = OSAppTheme.Unspecified;
                    Application.Current.UserAppTheme = OSAppTheme.Unspecified;
                }
            }

            if (args.PropertyName == nameof(SelectedLang))
            {

                Resources.CultureChange(SelectedLang);
            }
        }



        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            Application.Current.UserAppTheme = (OSAppTheme)_settingsManager.Theme;
            Resources.CultureChange(_settingsManager.Lang);
        }

        #endregion

    }
}
