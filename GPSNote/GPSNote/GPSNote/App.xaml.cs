using GPSNote.Servcies.Authentication;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.PinService;
using GPSNote.Servcies.Repository;
using GPSNote.Servcies.Settings;
using GPSNote.Servcies.ThemeService;
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

        private IThemeService _Theme;
        private IThemeService ThemeService => _Theme ?? (_Theme = Container.Resolve<IThemeService>());

        private IAuthorizationService _AuthorizationService;
        private IAuthorizationService AuthorizationService =>
            _AuthorizationService ?? (_AuthorizationService = Container.Resolve<IAuthorizationService>());

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Application.Current.UserAppTheme = (OSAppTheme)ThemeService.Theme;

            if (AuthorizationService.IsAutorized)
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(SignIn)}");
            }
            else
            {
                await NavigationService.NavigateAsync($"/{nameof(TabbedPage1)}");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IThemeService>(Container.Resolve<ThemeService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationService>());
            containerRegistry.RegisterInstance<ILocalizationService>(Container.Resolve<LocalizationService>()); 
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
            
            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignIn, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUp, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<Settings, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<PinList, PinListViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPin, AddEditPinViewModel>();
            containerRegistry.RegisterForNavigation<TabbedPage1, TabbedPage1ViewModel>();
        }
    }
}
