using GPSNote.Models;
using GPSNote.ViewModels.ExtendedViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Extensions
{
    public static class PinExtensions
    {
        public static Pin ToPin(this PinViewModel pinViewModel)
        {
            var result = new Pin();


            return result;
        }

        public static PinViewModel ToPin(this Pin pin)
        {
            var result = new PinViewModel();

            return result;
        }


        public static Pin ToPin(this UserPins pinViewModel)
        {
            var result = new Pin();

            result.Label = pinViewModel.Label;
            result.IsVisible = pinViewModel.IsEnabled;
            result.Position = new Position(pinViewModel.Latitude, pinViewModel.Longitude);

            return result;
        }

        public static UserPins ToUserPin(this Pin pinViewModel)
        {
            var result = new UserPins();



            return result;
        }

    }
}
