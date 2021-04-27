using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;
using Firebase;
using Xamarin.Forms.Platform.Android.AppLinks;
using Android.Content;
using Prism.Plugin.Popups;

namespace GPSNote.Droid
{
    [Activity(Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation=ScreenOrientation.Portrait)]
    [IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
              DataScheme = "https",
              DataHost = "gpsnote.page.link",
              DataPathPrefix = "/pin",
              AutoVerify = true)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            FirebaseApp.InitializeApp(this);
            AndroidAppLinks.Init(this);

            Rg.Plugins.Popup.Popup.Init(this);

            UserDialogs.Init(this);
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }

    //public override void OnBackPressed()
    //{
    //    PopupPlugin.OnBackPressed();
    //}


    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations

            containerRegistry.RegisterPopupNavigationService();
        }
    }
}

