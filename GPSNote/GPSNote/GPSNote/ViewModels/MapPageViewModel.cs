using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.Settings;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {

        #region _______Services______

        private readonly IPageDialogService _pageDialogService; 
        private readonly ISettingsManager _settingsManager;

        #endregion





        public MapPageViewModel(INavigationService navigationService, ILocalizationService localizationService, IPageDialogService pageDialogService, ISettingsManager settingsManager)
            : base(navigationService, localizationService)
        {

            _pageDialogService = pageDialogService;
            _settingsManager = settingsManager;

            CameraPos = "-23.68, -46.87, 13";

        }



        #region ______Public Properties______


        private string _CameraPos;
        public string CameraPos
        {
            get { return _CameraPos; }
            set { SetProperty(ref _CameraPos, value); }
        }

        #endregion



        public ICommand ItemTappedCommand => new Command(ExecutedTapCommand);

        public ICommand MapMovedCommand => new Command(ExecutedMapMovedCommand);





        private async void ExecutedTapCommand(object p)
        {
            Position position = (Position)p;

            await _pageDialogService.DisplayAlertAsync("It's", $"{position.Latitude} {position.Longitude}", "Ok");

        }


 

        private async void ExecutedMapMovedCommand(object p)
        {
            CameraPosition position = (CameraPosition)p;


            await _pageDialogService.DisplayAlertAsync("It's", $"{position.Target.Latitude} {position.Target.Longitude} {position.Zoom}", "Ok");
            _settingsManager.Longitude = position.Target.Latitude;
            _settingsManager.Longitude = position.Target.Longitude;
            _settingsManager.Zoom = position.Zoom;
        }








    }
}
