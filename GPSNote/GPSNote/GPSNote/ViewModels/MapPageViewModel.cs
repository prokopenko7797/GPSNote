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

        private async void ExecutedTapCommand(object p)
        {
            Position position = (Position)p;

            await _pageDialogService.DisplayAlertAsync("It's", $"{position.Latitude} {position.Longitude}", "Ok");

        }



        public ICommand MapMovedCommand => new Command(ExecutedMapMovedCommand);
        private async void ExecutedMapMovedCommand(object p)
        {
            CameraPosition position = (CameraPosition)p;


            await _pageDialogService.DisplayAlertAsync("It's", $"{position.Target.Latitude} {position.Target.Longitude} {position.Zoom}", "Ok");
            _settingsManager.Longitude = position.Target.Latitude;
            _settingsManager.Longitude = position.Target.Longitude;
            _settingsManager.Zoom = position.Zoom;


        }

        private MapType _type = MapType.Hybrid;

        public MapType type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }






        Pin a = new Pin()
        {
            Type = PinType.Place,
            Label = "Central Park NYC",
            Address = "New York City, NY 10022",
            Position = new Position(40.78d, -73.96d),
            Tag = "id_new_york"
        };

        IList<Pin> list = new List<Pin>();

        private List<Pin> _pins = new List<Pin>();

        public List<Pin> pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }


        public ICommand ChangeMapTypeCommand => new Command(ChangeMapType);




        private void ChangeMapType(object p)
        {
            type = MapType.Satellite;

        }



    }
}
