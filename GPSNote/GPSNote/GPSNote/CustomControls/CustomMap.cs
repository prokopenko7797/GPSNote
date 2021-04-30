using GPSNote.ViewModels.ExtendedViewModels;
using GPSNote.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.CustomControls
{
    public class CustomMap : Map
    {

        public CustomMap() 
        {
            UiSettings.CompassEnabled = true;
            MyLocationEnabled = true;
            UiSettings.MyLocationButtonEnabled = true;
        }

        #region -- Public properties --

        public static readonly BindableProperty ObsPinsProperty = BindableProperty.Create(
            propertyName: nameof(ObsPins),
            returnType: typeof(ObservableCollection<PinViewModel>),
            declaringType: typeof(CustomMap),
            defaultValue: default,
            propertyChanged: ObsPinsPropertyChanged);



        public ObservableCollection<PinViewModel> ObsPins
        {
            get { return (ObservableCollection<PinViewModel>)GetValue(ObsPinsProperty); }
            set { SetValue(ObsPinsProperty, value); }
        }


        public static readonly BindableProperty MoveToProperty = BindableProperty.Create(
            propertyName: nameof(MoveTo),
            returnType: typeof(MapSpan),
            declaringType: typeof(CustomMap),
            defaultValue: default,
            propertyChanged: MoveToPropertyChanged);



        public MapSpan MoveTo
        {
            get { return (MapSpan)GetValue(MoveToProperty); }
            set { SetValue(MoveToProperty, value); }
        }
        #endregion

        #region -- Private Helpers --

        private static void ObsPinsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomMap)bindable;
            control.Pins.Clear();


            if (newValue != null)
            {
                foreach (PinViewModel pin in (ObservableCollection<PinViewModel>)newValue)
                {
                    control.Pins.Add(pin.ToPin());
                }
            }
    
        }

        private static void MoveToPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomMap)bindable;
            control.MoveToRegion((MapSpan)newValue);

        }

        #endregion
    }
}
