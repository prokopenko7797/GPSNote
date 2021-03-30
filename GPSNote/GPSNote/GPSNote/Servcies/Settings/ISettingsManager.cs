using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Servcies.Settings
{
    public interface ISettingsManager
    {
        string Lang { get; set; }
        int Theme { get; set; }
    }
}
