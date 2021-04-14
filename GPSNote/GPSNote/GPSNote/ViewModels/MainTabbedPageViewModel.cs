using GPSNote.Servcies.LocalizationService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSNote.ViewModels
{
    public class MainTabbedPageViewModel : ViewModelBase
    {
        public MainTabbedPageViewModel(INavigationService navigationService, ILocalizationService localizationService) : base(navigationService, localizationService)
        {

        }
    }
}
