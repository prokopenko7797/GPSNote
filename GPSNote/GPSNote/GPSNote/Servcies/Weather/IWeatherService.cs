using Awesomio.Weather.NET.Models.OneCall;
using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.Weather
{
    public interface IWeatherService
    {

        Task<OneCallModel> GetOneCallForecast(double latitude, double longtitude);
    }
}