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

        #region -- ObsPins property --

        public static readonly BindableProperty ObsPinsProperty = BindableProperty.Create(
            propertyName: nameof(ObsPins),
            returnType: typeof(ObservableCollection<PinViewModel>),
            declaringType: typeof(CustomMap),
            defaultValue: default,
            propertyChanged: ObsPinsPropertyChanged);

        private static void ObsPinsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomMap)bindable;
            control.Pins.Clear();
            foreach (PinViewModel pin in (ObservableCollection<PinViewModel>)newValue)
            {
                control.Pins.Add(pin.ToPin());
            }
        }

        public ObservableCollection<PinViewModel> ObsPins
        {
            get { return (ObservableCollection<PinViewModel>)GetValue(ObsPinsProperty); }
            set { SetValue(ObsPinsProperty, value); }
        }

        #endregion

        #region -- MoveTo property --

        public static readonly BindableProperty MoveToProperty = BindableProperty.Create(
            propertyName: nameof(MoveTo),
            returnType: typeof(MapSpan),
            declaringType: typeof(CustomMap),
            defaultValue: default,
            propertyChanged: MoveToPropertyChanged);

        private static void MoveToPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomMap)bindable;
            control.MoveToRegion((MapSpan)newValue);

        }

        public MapSpan MoveTo
        {
            get { return (MapSpan)GetValue(MoveToProperty); }
            set { SetValue(MoveToProperty, value); }
        }

        #endregion
    }
}
