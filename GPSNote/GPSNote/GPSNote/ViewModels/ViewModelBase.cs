using GPSNote.Servcies.LocalizationService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }



        public ILocalizationService Resources
        {
            get;
            private set;
        }




        public ViewModelBase(INavigationService navigationService, ILocalizationService localizationService)
        {
            NavigationService = navigationService;
            Resources = localizationService;
        }




        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }


    }
}
