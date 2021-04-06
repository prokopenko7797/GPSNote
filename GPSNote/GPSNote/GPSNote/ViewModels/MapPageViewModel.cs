using GPSNote.CustomControls.CustomMap;
using GPSNote.Models;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
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
        private readonly IPinService _pinService;

        #endregion





        public MapPageViewModel(INavigationService navigationService, ILocalizationService localizationService, IPageDialogService pageDialogService, ISettingsManager settingsManager,
            IPinService pinService)
            : base(navigationService, localizationService)
        {

            _pageDialogService = pageDialogService;
            _settingsManager = settingsManager;
            _pinService = pinService;
        }




        public DelegateCommand<object> ItemTappedCommand => new DelegateCommand<object>(TapCommand);

        private async void TapCommand(object p)
        {
            Position position = (Position)p;

            //await _pageDialogService.DisplayAlertAsync("It's", $"{position.Latitude} {position.Longitude}", "Ok");



            Pin pin = new Pin()
            {
                Type = PinType.Place,
                Label = "Central Park NYC",
                Address = "New York City, NY 10022",
                Position = position,
                Tag = "id_new_york"
                
            };

            await _pinService.AddAsync(new UserPins(){ Latitude = position.Latitude, Longitude = position.Longitude, Label = "My pin"});

           

            pins = await _pinService.GetUserPinsAsync(); ;
            
        }





        public DelegateCommand<object> MapMovedCommand => new DelegateCommand<object>(ExecutedMapMovedCommand);
 
        private async void ExecutedMapMovedCommand(object p)
        {
            CameraPosition position = (CameraPosition)p;


          //  await _pageDialogService.DisplayAlertAsync("It's", $"{position.Target.Latitude} {position.Target.Longitude} {position.Zoom}", "Ok");



        }


        private MapType _type;

        public MapType type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private List<Pin> _pins;

        public List<Pin> pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }


        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            pins = await _pinService.GetUserPinsAsync();
        }
    }
}
