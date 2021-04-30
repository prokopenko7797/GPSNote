using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GPSNote
{
    public static class Constant
    {
        public const string DbName = "sqlite.db";
        public const int MinNameLength = 4;
        public const int MaxLoginLength = 16;
        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 16;
        public const int NonAuthorized = -1;
        public const int SQLError = -1;
        public const string DefaultLanguage = ResourcesLangConst.En;
        public const int DefaultTheme = (int)OSAppTheme.Light;
        public const string Email = nameof(Email);
        public const string Name = nameof(Name);
        public const string ImageFavorite = "favorite.png";
        public const string Host = "gpsnote.page.link";
        public const string Action = "pin";
        public const string WeatherAPIKey = "9600b0e0a8807fdc09ff9b5e467e5d71";
        public const double RomeLatitude = 41.89;
        public const double RomeLongtitude = 12.49;

        public static class ResourcesLangConst
        {
            public const string Ru = "ru";
            public const string En = "en-US";
        }

    }
}
