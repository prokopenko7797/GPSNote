using Acr.UserDialogs;
using GPSNote.Models;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.ViewModels
{
    public class AddEditPinViewModel : ViewModelBase
    {

   
        private readonly IPageDialogService _PageDialogService;
        private readonly IPinService _PinService;
        private readonly IAuthorizationService _authorizationService;

        private UserPins _UserPin = new UserPins();

        public AddEditPinViewModel(INavigationService navigationService,
                                ILocalizationService localizationService,
                                IPinService pinService,
                                IPageDialogService pageDialogService,
                                IAuthorizationService authorizationService)
                                : base(navigationService, localizationService)
        {
            _PinService = pinService;
            _authorizationService = authorizationService;
            _PageDialogService = pageDialogService;
            PinList = new List<Pin>();
        }





        #region -- Public properties --

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value); }
        }

        private double _Latitude;
        public double Latitude
        {
            get { return _Latitude; }
            set { SetProperty(ref _Latitude, value); }
        }

        private double _Longitude;
        public double Longitude
        {
            get { return _Longitude; }
            set { SetProperty(ref _Longitude, value); }
        }

        private List<Pin> _pins;
        public List<Pin> PinList
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        private DelegateCommand _SaveToolBarCommand;
        public DelegateCommand SaveToolBarCommand =>
            _SaveToolBarCommand ??
            (_SaveToolBarCommand = new DelegateCommand(ExecuteSaveToolBarAsync));


        public DelegateCommand<object> ItemTappedCommand => new DelegateCommand<object>(ExecutedItemTappedCommand);
        private void ExecutedItemTappedCommand(object p)
        {
            Position position = (Position)p;

            Latitude = position.Latitude;
            Longitude = position.Longitude;

            Pin pin = new Pin()
            {
                Label = "Your pin",
                Position = position
                
            };

            List<Pin> list = new List<Pin>();
            list.Add(pin);
            PinList = list;

        }

        #endregion


        #region ---Private Helpers---


        private async void ExecuteSaveToolBarAsync()
        {
            if (Name == default || Name.Length < 1)
            {
                await _PageDialogService.DisplayAlertAsync( Resources["NameEmpty"], Resources["Error"], Resources["Ok"]);
                ///////////////////////////////////
                return;
            }

            if (_UserPin.Label != Name || _UserPin.Latitude != Latitude
                    || _UserPin.Longitude != Longitude || _UserPin.Description != Description)
            {

                _UserPin.Label = Name;
                _UserPin.Description = Description;
                _UserPin.Latitude = Latitude;
                _UserPin.Longitude = Longitude;
                _UserPin.user_id = _authorizationService.IdUser;
                _UserPin.IsEnabled = true;

                if (_UserPin.id == default)
                {
                    await _PinService.AddPinAsync(_UserPin);
                }
                else
                {
                    await _PinService.EditPinAsync(_UserPin);
                }
            }
            await NavigationService.GoBackAsync();
        }

        #endregion





        #region --Overrides--

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetValue<UserPins>(nameof(UserPins)) != null)
            {
                _UserPin = parameters.GetValue<UserPins>(nameof(UserPins));
                Name = _UserPin.Label;
                Description = _UserPin.Description;
                Latitude = _UserPin.Latitude;
                Longitude = _UserPin.Longitude;

                Title = Resources["EditProfileTitle"];
            }
            else Title = Resources["AddProfileTitle"];

        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(Latitude) || args.PropertyName == nameof(Longitude))
            {
                Position position = new Position(Latitude, Longitude); 
                
                Pin pin = new Pin()
                {
                    Label = "Your pin",
                    Position = position
                };

                List<Pin> list = new List<Pin>();
                list.Add(pin);
                PinList = list;

            }
        }
    }

    #endregion
}
