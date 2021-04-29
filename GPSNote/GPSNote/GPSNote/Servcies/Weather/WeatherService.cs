using Awesomio.Weather.NET;
using Awesomio.Weather.NET.Enums;
using Awesomio.Weather.NET.Models.OneCall;
using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherClient _client;

        public WeatherService() 
        {
            _client = new WeatherClient(Constant.WeatherAPIKey);
        }

        #region -- IWeatherService implementation --

        public async Task<OneCallModel> GetOneCallForecast(double latitude, double longtitude)
        {
            OneCallModel oneCallModel = await _client.GetOneCallApiAsync<OneCallModel>(latitude, longtitude, "en");
            return oneCallModel;
        }
        #endregion
    }
}
