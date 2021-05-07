using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GPSNote.ViewModels
{
    public class LangSettingsViewModel : ViewModelBase
    {
        public LangSettingsViewModel(INavigationService navigationService, ILocalizationService localizationService, IPinService pinService)
            : base(navigationService, localizationService)
        {
            
        }

        #region -- Public properties --

        private string _SelectedLang;

        public string SelectedLang
        {
            get { return _SelectedLang; }
            set { SetProperty(ref _SelectedLang, value); }
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


        private DelegateCommand _BackButtonCommand;
        public DelegateCommand BackButtonCommand =>
            _BackButtonCommand ??
            (_BackButtonCommand = new DelegateCommand(OnBackButtonCommand));



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


        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsCheckedEn))
            {
                if (IsCheckedEn)
                {
                    Resources.ChangeCulture(Constant.ResourcesLangConst.En);
                    Resources.Lang = Constant.ResourcesLangConst.En;
                }
                else
                {
                    Resources.ChangeCulture(Constant.ResourcesLangConst.Ru);
                    Resources.Lang = Constant.ResourcesLangConst.Ru;
                }    
            }

    
        }



        #endregion

        #region -- Private helpers --



        private async void OnBackButtonCommand()
        {
            await NavigationService.GoBackAsync();
        }


        #endregion

    }
}

