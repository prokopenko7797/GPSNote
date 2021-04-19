using GPSNote.ViewModels.ExtendedViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GPSNote.Servcies.PinShare
{
    public class PinShareService : IPinShareService
    {
        #region -- IPinShare implementation --

        public async void PinShareAsync(PinViewModel pinViewModel)
        {
            string text = $"https://gpsnote.page.link/pin" +
                $"?{pinViewModel.Label}&{pinViewModel.Description}${pinViewModel.Latitude}&{pinViewModel.Longitude}";

            await Share.RequestAsync(new ShareTextRequest
            {
                Title = "SharePin",
                Text = text
            });
        }

        #endregion
    }
}
