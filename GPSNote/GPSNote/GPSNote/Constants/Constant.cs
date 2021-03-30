﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GPSNote.Constants
{
    public static class Constant
    {
        public const string DB_Name = "sqlite.db";
        public const int MinLoginLength = 4;
        public const int MaxLoginLength = 16;
        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 16;
        public const int NonAuthorized = -1;
        public const int SQLError = -1;
        public const string DefaultLanguage = ResourcesLangConst.en;
        public const int DefaultTheme = (int)OSAppTheme.Unspecified;
        public const string Email = "Email";



        public static class ResourcesLangConst
        {
            public const string ru = "ru";
            public const string en = "en-US";
        }

    }
}
