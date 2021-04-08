using Acr.UserDialogs;
using GPSNote.Models;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.ViewModels
{
    public class EditPinViewModel : ViewModelBase
    {
        private UserPins _userPin = new UserPins();

        private readonly IPinService _PinService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAuthorizationService _authorizationService;



        public EditPinViewModel(INavigationService navigationService,
                                ILocalizationService localizationService,
                                IPinService pinService,
                                IUserDialogs userDialogs,
                                IAuthorizationService authorizationService)
                                : base(navigationService, localizationService)
        {
            _PinService = pinService;
            _userDialogs = userDialogs;
            _authorizationService = authorizationService;
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
            Pin pin = new Pin()
            {
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
                await _userDialogs.AlertAsync(Resources["NickNameEmpty"], Resources["Error"], Resources["Ok"]);
                return;
            }

            if (_userPin.Label != Name || _userPin.Latitude != Latitude
                    || _userPin.Longitude != Longitude || _userPin.Description != Description)
            {

                _userPin.Label = Name;
                _userPin.Description = Description;
                _userPin.Latitude = Latitude;
                _userPin.Longitude = Longitude;
                _userPin.user_id = _authorizationService.IdUser;
                _userPin.IsEnabled = true;

                if (_userPin.id == default)
                {
                    await _PinService.AddPinAsync(_userPin);
                }
                else
                {
                    await _PinService.EditPinAsync(_userPin);
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
                _userPin = parameters.GetValue<UserPins>(nameof(UserPins));
                Name = _userPin.Label;
                Description = _userPin.Description;
                Latitude = _userPin.Latitude;
                Longitude = _userPin.Longitude;

                Title = Resources["EditProfileTitle"];
            }
            else Title = Resources["AddProfileTitle"];

        }



        #endregion

    }
}
