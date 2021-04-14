using GPSNote.Models;
using GPSNote.ViewModels.ExtendedViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Extensions
{
    public static class PinExtensions
    {
        public static Pin ToPin(this PinViewModel pinViewModel)
        {
            var result = new Pin();

            result.Label = pinViewModel.Label;
            result.IsVisible = pinViewModel.IsEnabled;
            result.Position = new Position(pinViewModel.Latitude, pinViewModel.Longitude);

            return result;
        }

        public static PinViewModel ToPinViewModel(this Pin pin)
        {
            var result = new PinViewModel();

            result.Label = pin.Label;
            result.IsEnabled = pin.IsVisible;
            result.Latitude = pin.Position.Latitude;
            result.Longitude = pin.Position.Longitude;

            
            return result;
        }

        public static PinModel ToPinModel(this PinViewModel pinViewModel)
        {
            var result = new PinModel();

            result.id = pinViewModel.Id;
            result.user_id = pinViewModel.UserId;
            result.Label = pinViewModel.Label;
            result.IsEnabled = pinViewModel.IsEnabled;
            result.Latitude = pinViewModel.Latitude;
            result.Longitude = pinViewModel.Longitude;
            result.Description = pinViewModel.Description;

            return result;
        }

        public static PinViewModel ToPinViewModel(this PinModel pin)
        {
            var result = new PinViewModel();

            result.Id = pin.id;
            result.UserId = pin.user_id;
            result.Label = pin.Label;
            result.IsEnabled = pin.IsEnabled;
            result.Latitude = pin.Latitude;
            result.Longitude = pin.Longitude;
            result.Description = pin.Description;


            return result;
        }

        public static Pin ToPin(this PinModel userPin)
        {
            var result = new Pin();

            result.Label = userPin.Label;
            result.IsVisible = userPin.IsEnabled;
            result.Position = new Position(userPin.Latitude, userPin.Longitude);

            return result;
        }

        public static List<Pin> ToListOfPin(this ObservableCollection<PinViewModel> obsPin) 
        {
            var result = new List<Pin>();

            foreach (var item in obsPin)
            {
               result.Add(item.ToPin());
            }

            return result;
        }

        public static ObservableCollection<PinViewModel> ToOpsOfPinView(this IEnumerable<PinModel> obsPin)
        {
            var result = new ObservableCollection<PinViewModel>();

            foreach (var item in obsPin)
            {
                result.Add(item.ToPinViewModel());
            }

            return result;
        }

        public static List<Pin> ToPinList(this IEnumerable<PinModel> obsPin)
        {
            var result = new List<Pin>();

            foreach (var item in obsPin)
            {
                result.Add(item.ToPin());
            }

            return result;
        }

    }
}
