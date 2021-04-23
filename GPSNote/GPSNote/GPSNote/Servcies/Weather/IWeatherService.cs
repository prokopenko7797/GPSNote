using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.Weather
{
    public interface IWeatherService
    {
        Task<CurrentWeatherResponse> GetCurrentWeatherAsync(double latitude, double longtitude);
    }
}