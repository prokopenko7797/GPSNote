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
    public partial class CustomMapView : ContentView
    {

        private static Position _lastPosition;
        private static double _lastZoom;
        private static double _lastBearing;
        private static double _lastTilt;




        public static readonly BindableProperty MapTypeProperty = BindableProperty.Create(
            propertyName: "MapType",
            returnType: typeof(MapType),
            declaringType: typeof(CustomMap),
            defaultValue: MapType.None,
            propertyChanged: MapTypePropertyChanged );

        private static void MapTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomMapView)bindable;
            control.map.MapType = (MapType)newValue;
        }

        public MapType MapType
        {
            get { return (MapType)GetValue(MapTypeProperty); }
            set { SetValue(MapTypeProperty, value); }
        }





        public static readonly BindableProperty PinsProperty = BindableProperty.Create(
            propertyName: "Pins",
            returnType: typeof(List<Pin>),
            declaringType: typeof(CustomMap),
            defaultValue: default,
            propertyChanged: PinsPropertyChanged);



        private static void PinsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomMapView)bindable;
            foreach (Pin pin in (List<Pin>)newValue)
                control.map.Pins.Add((Pin)newValue);
        }


        public List<Pin> Pins
        {
            get { return (List<Pin>)GetValue(PinsProperty); }
            set { SetValue(PinsProperty, value); }
        }







        public CustomMapView()
        {
            InitializeComponent();

        }
    }
}