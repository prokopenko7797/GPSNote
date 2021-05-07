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
            var result = new Pin()
            {
                Label = pinViewModel.Label,
                IsVisible = pinViewModel.IsEnabled,
                Position = new Position(pinViewModel.Latitude, pinViewModel.Longitude)
            };
            return result;
        }


        public static PinModel ToPinModel(this PinViewModel pinViewModel)
        {
            var result = new PinModel()
            {
                Id = pinViewModel.Id,
                UserId = pinViewModel.UserId,
                Label = pinViewModel.Label,
                IsEnabled = pinViewModel.IsEnabled,
                Latitude = pinViewModel.Latitude,
                Longitude = pinViewModel.Longitude,
                Description = pinViewModel.Description,
            };
            return result;
        }

        public static PinViewModel ToPinViewModel(this PinModel pin)
        {
            var result = new PinViewModel()
            {
                Id = pin.Id,
                UserId = pin.UserId,
                Label = pin.Label,
                IsEnabled = pin.IsEnabled,
                Latitude = pin.Latitude,
                Longitude = pin.Longitude,
                Description = pin.Description
            };
            return result;
        }


        public static ObservableCollection<PinViewModel> ToPinViewObservableCollection(this IEnumerable<PinModel> obsPin)
        {
            var result = new ObservableCollection<PinViewModel>();

            foreach (var item in obsPin)
            {
                result.Add(item.ToPinViewModel());
            }
            return result;
        }

        public static PinViewModel ToPinViewModel(this Uri uri)
        {
            var parts = uri.Query.Split('&');

            PinViewModel pin = new PinViewModel()
            {
                Label = parts[0].Replace("?", string.Empty),
                Description = parts[1],
                Latitude = Convert.ToDouble(parts[2]),
                Longitude = Convert.ToDouble(parts[3])
            };
            return pin;
        }
    }
}
