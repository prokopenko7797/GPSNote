using GPSNote.CustomControls.CustomMap;
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

        }




        public DelegateCommand<object> ItemTappedCommand => new DelegateCommand<object>(TapCommand);

        private async void TapCommand(object p)
        {
            Position position = (Position)p;

            await _pageDialogService.DisplayAlertAsync("It's", $"{position.Latitude} {position.Longitude}", "Ok");

            
        }



        public DelegateCommand<object> MapMovedCommand => new DelegateCommand<object>(ExecutedMapMovedCommand);
 
        private async void ExecutedMapMovedCommand(object p)
        {
            CameraPosition position = (CameraPosition)p;


            await _pageDialogService.DisplayAlertAsync("It's", $"{position.Target.Latitude} {position.Target.Longitude} {position.Zoom}", "Ok");



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

        private List<Pin> _pins;

        public List<Pin> pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }


        public DelegateCommand<object> ChangeMapTypeCommand => new DelegateCommand<object>(ChangeMapType);




        private void ChangeMapType(object p)
        {
            type = MapType.Satellite;
            List<Pin> test = new List<Pin>();
            test.Add(a);
            pins = test;

        }





    }
}
