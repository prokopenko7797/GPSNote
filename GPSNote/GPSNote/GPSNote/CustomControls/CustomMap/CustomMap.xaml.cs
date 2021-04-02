using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.CustomControls.CustomMap
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomMap : ContentView
    {
        public CustomMap()
        {
            InitializeComponent();


            editorMapStyle.Text =
               "[\n" +
               "  {\n" +
               "    \"elementType\": \"labels\",\n" +
               "    \"stylers\": [\n" +
               "      {\n" +
               "        \"visibility\": \"off\"\n" +
               "      }\n" +
               "    ]\n" +
               "  },\n" +
               "  {\n" +
               "    \"featureType\": \"water\",\n" +
               "    \"elementType\": \"geometry.fill\",\n" +
               "    \"stylers\": [\n" +
               "      {\n" +
               "        \"color\": \"#4c4c4c\"\n" +
               "      }\n" +
               "    ]\n" +
               "  }" +
               "]";

            buttonClearStyle.Clicked += (sender, e) =>
            {
                map.MapStyle = null;
            };

            buttonMapStyle.Clicked += (sender, e) =>
            {
                map.MapStyle = MapStyle.FromJson(editorMapStyle.Text);
            };



        }
    }
}