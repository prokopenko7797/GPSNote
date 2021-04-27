using GPSNote.Servcies.Authentication;
using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.LocalizationService;
using GPSNote.Servcies.Permission;
using GPSNote.Servcies.PinService;
using GPSNote.Servcies.PinShare;
using GPSNote.Servcies.Repository;
using GPSNote.Servcies.Settings;
using GPSNote.Servcies.ThemeService;
using GPSNote.ViewModels;
using GPSNote.ViewModels.ExtendedViewModels;
using GPSNote.Views;
using GPSNote.Extensions;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using System;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using GPSNote.Servcies.Weather;
using System.Threading.Tasks;
using Prism.Plugin.Popups;

namespace GPSNote
{
    public partial class App
    {

        private IThemeService _Theme;
        private IThemeService ThemeService => _Theme ?? (_Theme = Container.Resolve<IThemeService>());

        private IAuthorizationService _AuthorizationService;
        private IAuthorizationService AuthorizationService =>
            _AuthorizationService ?? (_AuthorizationService = Container.Resolve<IAuthorizationService>());

        private TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            ThemeService.SetTheme(ThemeService.GetCurrentTheme());


            if (AuthorizationService.IsAuthorized)
            {
                await NavigationService.NavigateAsync(nameof(MainPage));
            }
            else
            {
                await NavigationService.NavigateAsync(nameof(MainTabbedPage));
            }

            taskCompletionSource.SetResult(true);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IThemeService>(Container.Resolve<ThemeService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationService>());
            containerRegistry.RegisterInstance<ILocalizationService>(Container.Resolve<LocalizationService>()); 
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
            containerRegistry.RegisterInstance<IPermissionService>(Container.Resolve<PermissionService>());
            containerRegistry.RegisterInstance<IPinShareService>(Container.Resolve<PinShareService>());
            containerRegistry.RegisterInstance<IWeatherService>(Container.Resolve<WeatherService>());

            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignIn, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUp, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<Settings, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<PinList, PinListViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPin, AddEditPinViewModel>();
            containerRegistry.RegisterForNavigation<MainTabbedPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateAccount, CreateAccountViewModel>();
            containerRegistry.RegisterForNavigation<PinInfoPopup, PinInfoPopupViewModel>();


            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterPopupDialogService();
        }

        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            Task<bool> t1 = taskCompletionSource.Task;

            await t1;

            if (uri.Host.Equals(Constant.Host, StringComparison.OrdinalIgnoreCase))
            {
                if (uri.Segments != null && uri.Segments.Length == 2)
                {
                    if (uri.Segments[1] == Constant.Action)
                    {
                        if (AuthorizationService.IsAuthorized)
                        {
                            PinViewModel pinView = uri.ToPinViewModel();

                            pinView.Label.Replace("*", " ");
                            pinView.Description.Replace("*", " ");

                            var parametrs = new NavigationParameters
                            {
                                { nameof(PinViewModel),  pinView}
                            };
                            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(AddEditPin)}", parametrs);
                        }
                       
                    }
                }
            }
            base.OnAppLinkRequestReceived(uri);
        }
    }
}
