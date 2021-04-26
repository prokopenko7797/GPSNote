using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly OpenWeatherMapClient _Client;

        public WeatherService() 
        {
            _Client = new OpenWeatherMapClient(Constant.WeatherAPIKey);
        }

        #region -- IWeatherService implementation --

        public async Task<CurrentWeatherResponse> GetCurrentWeatherAsync(double latitude, double longtitude) 
        {
            return await _Client.CurrentWeather.GetByCoordinates(
                new Coordinates() { Latitude = latitude, Longitude = longtitude },
                MetricSystem.Metric, 
                OpenWeatherMapLanguage.RU);
        }



        public async Task<ForecastResponse> GetForecastWeatherAsync(double latitude, double longtitude)
        {
            return await _Client.Forecast.GetByCoordinates(
                new Coordinates() { Latitude = latitude, Longitude = longtitude }, 
                true,
                MetricSystem.Metric,
                OpenWeatherMapLanguage.RU,
                4);
        }
        #endregion
    }
}
