using GPSNote.CustomControls.CustomMap;
using GPSNote.Models;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
using GPSNote.Servcies.Settings;
using GPSNote.ViewModels.ExtendedViewModels;
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
using GPSNote.Extensions;

namespace GPSNote.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;
        private readonly IPinService _pinService;

        public MapPageViewModel(INavigationService navigationService, ILocalizationService localizationService, 
            IPageDialogService pageDialogService, IPinService pinService)
            : base(navigationService, localizationService)
        {
            _pageDialogService = pageDialogService;
            _pinService = pinService;
        }

        #region -- Public properties --

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

            await _pinService.AddPinAsync(new UserPins(){ Latitude = position.Latitude, Longitude = position.Longitude, Label = "My pin"});

            PinList = GetPins(await _pinService.GetUserPinsAsync());

        }


        public DelegateCommand<object> MapMovedCommand => new DelegateCommand<object>(ExecutedMapMovedCommand); 



        private MapType _type;

        public MapType type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private List<Pin> _pins;

        public List<Pin> PinList
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        #endregion

        #region --Private helpers--
        private List<Pin> GetPins(IEnumerable<UserPins> userPins)
        {
            List<Pin> pins = new List<Pin>();

            foreach (UserPins p in userPins)
            {
                Position position = new Position(p.Latitude, p.Longitude);

                pins.Add(new Pin() { Position = position, Label = p.Label, Tag = p.Tag });
            }
            return pins;
        }

        private async void ExecutedMapMovedCommand(object p)
        {
            CameraPosition position = (CameraPosition)p;

            //  await _pageDialogService.DisplayAlertAsync("It's", $"{position.Target.Latitude} {position.Target.Longitude} {position.Zoom}", "Ok");
            var pinViewModel = new PinViewModel();
            var xxx = pinViewModel.ToPin();
        }

        #endregion

        #region --Overrides--

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            PinList = GetPins( await _pinService.GetUserPinsAsync());
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue<List<Pin>>(nameof(this.PinList), out var newPinsValue))
            {
                this.PinList = newPinsValue;
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            parameters.Add(nameof(this.PinList), this.PinList);
        }

        #endregion




    }
}
