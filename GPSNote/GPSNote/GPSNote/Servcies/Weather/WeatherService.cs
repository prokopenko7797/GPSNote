using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.Weather
{
    public class WeatherService : IWeatherService
    {
        private OpenWeatherMapClient _Client = new OpenWeatherMapClient("9600b0e0a8807fdc09ff9b5e467e5d71");

        #region -- IWeatherService implementation --

        public async Task<CurrentWeatherResponse> GetCurrentWeatherAsync(double latitude, double longtitude) 
        {
            return await _Client.CurrentWeather.GetByCoordinates(
                new Coordinates() { Latitude = latitude, Longitude = longtitude },
                MetricSystem.Metric, 
                OpenWeatherMapLanguage.RU);
        }

        #endregion
    }
}
