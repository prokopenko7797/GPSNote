﻿using Acr.UserDialogs;
using GPSNote.Models;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
using GPSNote.ViewModels.ExtendedViewModels;
using GPSNote.Extensions;
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

        private PinViewModel _pinViewModel = new PinViewModel();

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
            _SaveToolBarCommand ?? (_SaveToolBarCommand = new DelegateCommand(OnSaveToolBarAsync));


        private DelegateCommand<object> _ItemTappedCommand;
        public DelegateCommand<object> ItemTappedCommand => 
            _ItemTappedCommand ?? (_ItemTappedCommand = new DelegateCommand<object>(OnItemTappedCommand));

        #endregion


        #region ---Private Helpers---

        private void OnItemTappedCommand(object sender)
        {
            Position position = (Position)sender;

            Latitude = position.Latitude;
            Longitude = position.Longitude;


            Pin pin = new Pin()
            {
                Label = Resources["NewPin"],
                Position = position

            };

            List<Pin> list = new List<Pin>();
            list.Add(pin);
            PinList = list;

        }

        private async void OnSaveToolBarAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await _PageDialogService.DisplayAlertAsync( Resources["NameEmpty"], Resources["Error"], Resources["Ok"]);
                return;
            }

            if (_pinViewModel.Label != Name || _pinViewModel.Latitude != Latitude
                    || _pinViewModel.Longitude != Longitude || _pinViewModel.Description != Description)
            {
                _pinViewModel.Label = Name;
                _pinViewModel.Description = Description;
                _pinViewModel.Latitude = Latitude;
                _pinViewModel.Longitude = Longitude;
                _pinViewModel.UserId = _authorizationService.IdUser;
                _pinViewModel.IsEnabled = true;

                if (_pinViewModel.Id == default)
                {
                    await _PinService.AddPinAsync(_pinViewModel.ToUserPin()); 
                }
                else
                {
                    await _PinService.EditPinAsync(_pinViewModel.ToUserPin());
                }

                bool IsUpdated = true;

                var parametrs = new NavigationParameters
                {
                    { nameof(IsUpdated), IsUpdated }
                };
                await NavigationService.GoBackAsync(parametrs);
            }
        }

        #endregion


        #region --Overrides--

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetValue<PinViewModel>(nameof(PinViewModel)) != null)
            {
                _pinViewModel = parameters.GetValue<PinViewModel>(nameof(PinViewModel));
                Name = _pinViewModel.Label;
                Description = _pinViewModel.Description;
                Latitude = _pinViewModel.Latitude;
                Longitude = _pinViewModel.Longitude;

                Title = Resources["EditPinTitle"];
            }
            else Title = Resources["AddPinTitle"];

        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(Latitude) || args.PropertyName == nameof(Longitude))
            {
                Position position = new Position(Latitude, Longitude); 
                
                Pin pin = new Pin()
                {
                    Label = Resources["NewPin"],
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
