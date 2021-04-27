using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace GPSNote.Views
{
    public partial class MainTabbedPage : Xamarin.Forms.TabbedPage
    {
        public MainTabbedPage()
        {
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            InitializeComponent();
        }
    }
}
