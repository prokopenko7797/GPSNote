using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GPSNote.Servcies.ThemeService
{
    public interface IThemeService
    {

        OSAppTheme GetCurrentTheme();

        void SetTheme(OSAppTheme theme);
    }
}
