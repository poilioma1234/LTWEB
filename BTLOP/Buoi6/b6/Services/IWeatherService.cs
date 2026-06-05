using b6.Models;

namespace b6.Services
{
    public interface IWeatherService
    {
        Task<WeatherResultViewModel> GetForecastAsync(string query);
    }
}
