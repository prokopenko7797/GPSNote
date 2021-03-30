using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Servcies.LocalizationService
{
    public interface ILocalizationService
    {
        string this[string key]
        {
            get;
        }

        void CultureChange(string lang);
    }
}
