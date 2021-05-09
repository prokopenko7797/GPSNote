using Acr.UserDialogs;
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
using System.Threading.Tasks;
using OpenWeatherMap;
using System.Collections.ObjectModel;
using System.Globalization;

namespace GPSNote.ViewModels
{
    public class AddEditPinViewModel : ViewModelBase
    {

   
        private readonly IPageDialogService _PageDialogService;
        private readonly IPinService _PinService;
        private readonly IAuthorizationService _authorizationService;

        private PinViewModel _pinViewModel;

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
            
            _pinViewModel = new PinViewModel();

            ObsPins = new ObservableCollection<PinViewModel>();

            

            Title = Resources["AddPinTitle"];
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

        private string _Latitude;
        public string Latitude
        {
            get { return _Latitude; }
            set { SetProperty(ref _Latitude, value); }
        }

        private string _Longtitude;
        public string Longtitude
        {
            get { return _Longtitude; }
            set { SetProperty(ref _Longtitude, value); }
        }

        private ObservableCollection<PinViewModel> _obsPins;
        public ObservableCollection<PinViewModel> ObsPins
        {
            get { return _obsPins; }
            set { SetProperty(ref _obsPins, value); }
        }

        private DelegateCommand _SaveToolBarCommand;
        public DelegateCommand SaveToolBarCommand =>
            _SaveToolBarCommand ?? (_SaveToolBarCommand = new DelegateCommand(OnSaveToolBarAsync));


        private DelegateCommand<object> _ItemTappedCommand;
        public DelegateCommand<object> ItemTappedCommand => 
            _ItemTappedCommand ?? (_ItemTappedCommand = new DelegateCommand<object>(OnItemTappedCommand));

        private DelegateCommand _BackButtonCommand;
        public DelegateCommand BackButtonCommand =>
            _BackButtonCommand ??
            (_BackButtonCommand = new DelegateCommand(OnBackButtonCommand));

        #endregion

        #region --Overrides--

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinViewModel>(nameof(PinViewModel), out var newPinVM))
            {
                _pinViewModel = newPinVM;
                Name = _pinViewModel.Label;
                Description = _pinViewModel.Description;
                Latitude = _pinViewModel.Latitude.ToString();
                Longtitude = _pinViewModel.Longitude.ToString();

                Title = Resources["EditPinTitle"];

                ObsPins = new ObservableCollection<PinViewModel>();
            }


        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(Latitude) || args.PropertyName == nameof(Longtitude))
            {
                if (ObsPins.Count == 1)
                {
                    if (ObsPins.First().Latitude.ToString() != Latitude
                    || ObsPins.First().Longitude.ToString() != Longtitude)
                    {
                        UpdatePin();
                    }
                }
                else
                {
                    UpdatePin();
                }
                
            }
        }

        #endregion

        #region ---Private Helpers---

        private async void OnBackButtonCommand()
        {
            await NavigationService.GoBackAsync();
        }


        private void OnItemTappedCommand(object sender)
        {
            Position position = (Position)sender;


            Latitude = position.Latitude.ToString();
            Longtitude = position.Longitude.ToString();


        }


        private void UpdatePin() 
        {
            if (Latitude != null && Longtitude != null)
            {
                PinViewModel pinViewModel = new PinViewModel()
                {
                    Label = Resources["NewPin"],
                    Latitude = Convert.ToDouble(Latitude),
                    Longitude = Convert.ToDouble(Longtitude),
                    IsEnabled = true
                };



                ObservableCollection<PinViewModel> newPins = new ObservableCollection<PinViewModel>();
                newPins.Add(pinViewModel);

                ObsPins = newPins;
            }
  
        }

        private async void OnSaveToolBarAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await _PageDialogService.DisplayAlertAsync( Resources["NameEmpty"], Resources["Error"], Resources["Ok"]);
                return;
            }
            else
            {
                _pinViewModel.Label = Name;
                _pinViewModel.Description = Description;
                _pinViewModel.Latitude = Convert.ToDouble(Latitude);
                _pinViewModel.Longitude = Convert.ToDouble(Longtitude);
                _pinViewModel.UserId = _authorizationService.IdUser;
                _pinViewModel.IsEnabled = true;

                if (_pinViewModel.Id == default)
                {
                    await _PinService.AddPinModelAsync(_pinViewModel.ToPinModel());
                }
                else
                {
                    await _PinService.EditPinModelAsync(_pinViewModel.ToPinModel());
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
    }
}
