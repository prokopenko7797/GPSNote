using Acr.UserDialogs;
using GPSNote.Models;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
using GPSNote.Views;
using GPSNote.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Prism.Navigation.TabbedPages;
using System.ComponentModel;

namespace GPSNote.ViewModels
{
    public class PinListViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IAuthorizationService _authorizationService;

        private ObservableCollection<UserPins> _Current; 

        public PinListViewModel(INavigationService navigationService, ILocalizationService localizationService, IPinService pinService,
            IAuthorizationService authorizationService)
            : base(navigationService, localizationService)
        {
            _authorizationService = authorizationService;
            _pinService = pinService;
        }

        #region -- Public properties --
        private ObservableCollection<UserPins> _pinList;
        public ObservableCollection<UserPins> PinObs
        {
            get { return _pinList; }
            set { SetProperty(ref _pinList, value); }
        }

        private bool _IsVisible;
        public bool IsVisible
        {
            get { return _IsVisible; }
            set { SetProperty(ref _IsVisible, value); }
        }


        private UserPins _SelectedItem;
        public UserPins SelectedItem
        {
            get { return _SelectedItem; }
            set { SetProperty(ref _SelectedItem, value); }
        }


        string _SearchBarText;
        public string SearchBarText
        {
            get { return _SearchBarText; }
            set { SetProperty(ref _SearchBarText, value); }
        }

        private DelegateCommand<object> _LogOutToolBarCommand;
        public DelegateCommand<object> LogOutToolBarCommand =>
            _LogOutToolBarCommand ?? (_LogOutToolBarCommand = new DelegateCommand<object>(NavigateLogOutToolBarCommand));
       
        private DelegateCommand<object> _SettingsToolBarCommand;
        public DelegateCommand<object> SettingsToolBarCommand =>
            _SettingsToolBarCommand ?? (_SettingsToolBarCommand = new DelegateCommand<object>(NavigateSettingsCommand));

        private DelegateCommand<object> _AddEditButtonClicked;
        public DelegateCommand<object> AddEditButtonClicked =>
            _AddEditButtonClicked ?? (_AddEditButtonClicked = new DelegateCommand<object>(NavigateAddEditProfileCommand));


        private DelegateCommand<object> _EditCommandTap;
        public DelegateCommand<object> EditCommandTap => 
            _EditCommandTap ?? (_EditCommandTap = new DelegateCommand<object>(EditCommand));

        private DelegateCommand<object> _DeleteCommandTap;
        public DelegateCommand<object> DeleteCommandTap => 
            _DeleteCommandTap ?? (_DeleteCommandTap = new DelegateCommand<object>(DeleteCommand));

        private DelegateCommand<object> _ImageCommandTap;
        public DelegateCommand<object> ImageCommandTap => 
            _ImageCommandTap ?? (_ImageCommandTap = new DelegateCommand<object>(ChangeVisibilityComand));

        private DelegateCommand<object> _OnTextChangedCommand;
        public DelegateCommand<object> OnTextChangedCommand =>
            _OnTextChangedCommand ?? (_OnTextChangedCommand = new DelegateCommand<object>(SearchCommand));


        #endregion



        #region -- Private helpers --


        private void SearchCommand(object sender)
        {
            if (string.IsNullOrWhiteSpace(SearchBarText))
            {
                PinObs = _Current;
            }
            else
            {
                string low = SearchBarText.ToLower();


                PinObs = new ObservableCollection<UserPins>(_Current.Where(pin => (pin.Label.ToLower()).Contains(low) ||
                                                                                  (pin.Description.ToLower()).Contains(low) ||
                                                                                  (pin.Latitude.ToString()).Contains(low) ||
                                                                                  (pin.Longitude.ToString()).Contains(low)));
            }
        }

 

        private async void DeleteCommand(object sender)
        {
            if (!(sender is UserPins userPins)) return;

            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Message = Resources["Delete?"],
                OkText = Resources["Yes"],
                CancelText = Resources["No"]
            });
            if (result)
            {
                await _pinService.DeletePinAsync(userPins.id);
                PinObs.Remove(userPins);
                _Current.Remove(userPins);
                if (PinObs.Count() == 0) IsVisible = true;
            }
        }


        private async void EditCommand(object sender)
        {
            UserPins pins = sender as UserPins;
            
            var parametrs = new NavigationParameters
            {
                { nameof(UserPins), pins }
            };
            
            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(AddEditPin)}", parametrs);
        }


        private async void ChangeVisibilityComand(object sender)
        {
            UserPins pin = sender as UserPins;

            int i = PinObs.IndexOf(pin);
            int j = _Current.IndexOf(pin);
            pin.IsEnabled = !pin.IsEnabled;
            PinObs[i] = pin;
            _Current[j] = pin;

            await _pinService.EditPinAsync(pin);           
        }

        private async void NavigateAddEditProfileCommand(object sender)
        {
            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(AddEditPin)}");

        }

        private async void NavigateSettingsCommand(object sender)
        {
            await NavigationService.NavigateAsync(nameof(Settings));
        }

        private async void NavigateLogOutToolBarCommand(object sender)
        {
            _authorizationService.LogOut();
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignIn)}");
        }

        private async void UpdateCollectionAsync()
        {
            PinObs = new ObservableCollection<UserPins>(await _pinService.GetUserPinsAsync());
            _Current = PinObs;
            IsVisible = PinObs.Count() == 0;
        }

        #endregion

        #region --Overrides--

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            bool IsUpdated = false;

            if (parameters.TryGetValue<bool>(nameof(IsUpdated), out var newUpdate))
            {
                if (newUpdate)
                {
                    UpdateCollectionAsync();
                }
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            UpdateCollectionAsync();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            parameters.Add(nameof(PinObs), _Current);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(SelectedItem)) 
            {
                var parameters = new NavigationParameters
                {
                    { nameof(SelectedItem), SelectedItem }
                };

                NavigationService.SelectTabAsync($"{nameof(MapPage)}", parameters);
            }
        }

        #endregion
    }
}

