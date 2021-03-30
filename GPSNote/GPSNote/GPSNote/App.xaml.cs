using GPSNote.Servcies.AutorizationService;
using GPSNote.Servcies.RegistrationService;
using GPSNote.Servcies.Repository;
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

        #region -----Services----

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

            if (AuthorizationService.IsAuthorize())
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(SignIn)}");
    //////////        else await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainList)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IRegistrationService>(Container.Resolve<RegistrationService>());




            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SignIn, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUp, SignUpViewModel>();
        }
    }
}
