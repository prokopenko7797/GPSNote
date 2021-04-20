using GPSNote.Servcies.LocalizationService;
using GPSNote.ViewModels.ExtendedViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GPSNote.Servcies.PinShare
{
    public class PinShareService : IPinShareService
    {
        ILocalizationService _localizationService;

        public PinShareService(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        #region -- IPinShare implementation --

        public async void PinShareAsync(PinViewModel pinViewModel)
        {
            string text = $"https://gpsnote.page.link/pin" +
                $"?{pinViewModel.Label}&{pinViewModel.Description}${pinViewModel.Latitude}&{pinViewModel.Longitude}";

            await Share.RequestAsync(new ShareTextRequest
            {
                Title = _localizationService["SharingTitle"],
                Text = pinViewModel.Label,
                Uri = text
            });
        }

        #endregion
    }
}
