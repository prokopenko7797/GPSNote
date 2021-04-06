using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
using GPSNote.Servcies.RegistrationService;
using GPSNote.Servcies.Repository;
using GPSNote.Servcies.Settings;
using GPSNote.ViewModels;
using GPSNote.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace GPSNote
{
    public partial class App
    {

        #region -- IterfaceName implementation --

        private ISettingsManager _SettingsManager;
        private ISettingsManager SettingsManager =>
            _SettingsManager ?? (_SettingsManager = Container.Resolve<ISettingsManager>());

        private IAuthorizationService _AuthorizationService;
        private IAuthorizationService AuthorizationService =>
            _AuthorizationService ?? (_AuthorizationService = Container.Resolve<IAuthorizationService>());

        #endregion


        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Application.Current.UserAppTheme = (OSAppTheme)SettingsManager.Theme;

            //await NavigationService.NavigateAsync($"{nameof(TabbedPage1)}");
            if (AuthorizationService.IsAutorized)
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(SignIn)}");
            }
            else await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(TabbedPage1)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IRegistrationService>(Container.Resolve<RegistrationService>());
            containerRegistry.RegisterInstance<ILocalizationService>(Container.Resolve<LocalizationService>()); 
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignIn, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUp, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<Settings, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<TabbedPage1, TabbedPage1ViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<PinList, PinListViewModel>();
        }
    }
}
