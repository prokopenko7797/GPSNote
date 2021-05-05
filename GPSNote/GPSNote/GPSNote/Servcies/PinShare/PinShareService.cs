using GPSNote.Models;
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
        private readonly ILocalizationService _localizationService;

        public PinShareService(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        #region -- IPinShare implementation --

        public async void SharePinAsync(PinModel pinModel)
        {
            string text = $"https://gpsnote.page.link/pin" +
                $"?{pinModel.Label.Replace(" ","*")}&{pinModel.Description.Replace(" ", "*")}&{pinModel.Latitude}&{pinModel.Longitude}";

            await Share.RequestAsync(new ShareTextRequest
            {
                Title = _localizationService["SharingTitle"],
                Uri = text
            });
        }

        #endregion
    }
}
