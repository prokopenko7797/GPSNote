using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.CustomControls.CustomMap
{
    class CustomMap : Map
    {

        public static readonly BindableProperty PinsSelectProperty = BindableProperty.Create(
            propertyName: nameof(PinsSelect),
            returnType: typeof(List<Pin>),
            declaringType: typeof(CustomMap),
            defaultValue: default,
            propertyChanged: PinsSelectPropertyChanged);

        private static void PinsSelectPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomMap)bindable;
            control.Pins.Clear();
            foreach (Pin pin in (List<Pin>)newValue)
            { 
                control.Pins.Add(pin); 
            }
        }

        public List<Pin> PinsSelect
        {
            get { return (List<Pin>)GetValue(PinsSelectProperty); }
            set { SetValue(PinsSelectProperty, value); }
        }

    }
}
