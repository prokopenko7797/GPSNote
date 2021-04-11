using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Servcies.LastPositionService
{
    public interface ILastPositionService
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        double Zoom { get; set; }
    }
}
