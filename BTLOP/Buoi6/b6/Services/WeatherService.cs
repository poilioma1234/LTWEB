using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using b6.Models;

namespace b6.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WeatherResultViewModel> GetForecastAsync(string query)
        {
            try
            {
                return await GetWeatherApiForecastAsync(query);
            }
            catch (HttpRequestException)
            {
                return await GetOpenMeteoForecastAsync(query);
            }
        }

        private async Task<WeatherResultViewModel> GetWeatherApiForecastAsync(string query)
        {
            var apiKey = _configuration["WeatherApi:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("Chưa cấu hình WeatherAPI key.");
            }

            var url = $"forecast.json?key={Uri.EscapeDataString(apiKey)}&q={Uri.EscapeDataString(query.Trim())}&days=5&aqi=no&alerts=no&lang=vi";
            var response = await _httpClient.GetFromJsonAsync<WeatherApiResponse>(url);

            if (response?.Location is null || response.Current is null || response.Forecast?.ForecastDay is null)
            {
                throw new InvalidOperationException("Không đọc được dữ liệu thời tiết từ API.");
            }

            return new WeatherResultViewModel
            {
                LocationName = $"{response.Location.Name}, {response.Location.Country}",
                TemperatureC = response.Current.TempC,
                    Condition = response.Current.Condition?.Text ?? "Không rõ",
                IconUrl = NormalizeIconUrl(response.Current.Condition?.Icon),
                Humidity = response.Current.Humidity,
                WindKph = response.Current.WindKph,
                ForecastDays = response.Forecast.ForecastDay.Select(day => new ForecastDayViewModel
                {
                    Date = DateOnly.Parse(day.Date, CultureInfo.InvariantCulture),
                    MinTemperatureC = day.Day.MinTempC,
                    MaxTemperatureC = day.Day.MaxTempC,
                    Condition = day.Day.Condition?.Text ?? "Không rõ",
                    IconUrl = NormalizeIconUrl(day.Day.Condition?.Icon)
                }).ToList()
            };
        }

        private async Task<WeatherResultViewModel> GetOpenMeteoForecastAsync(string query)
        {
            var location = await ResolveOpenMeteoLocationAsync(query.Trim());
            var forecastUrl = "https://api.open-meteo.com/v1/forecast"
                + $"?latitude={location.Latitude.ToString(CultureInfo.InvariantCulture)}"
                + $"&longitude={location.Longitude.ToString(CultureInfo.InvariantCulture)}"
                + "&current=temperature_2m,relative_humidity_2m,wind_speed_10m,weather_code"
                + "&daily=weather_code,temperature_2m_max,temperature_2m_min"
                + "&forecast_days=5&timezone=auto";

            var response = await _httpClient.GetFromJsonAsync<OpenMeteoForecastResponse>(forecastUrl);
            if (response?.Current is null || response.Daily is null)
            {
                throw new InvalidOperationException("Không đọc được dữ liệu thời tiết từ Open-Meteo.");
            }

            var forecastDays = new List<ForecastDayViewModel>();
            for (var i = 0; i < response.Daily.Time.Count; i++)
            {
                var code = response.Daily.WeatherCode.ElementAtOrDefault(i);
                forecastDays.Add(new ForecastDayViewModel
                {
                    Date = DateOnly.Parse(response.Daily.Time[i], CultureInfo.InvariantCulture),
                    MinTemperatureC = response.Daily.MinTemperature.ElementAtOrDefault(i),
                    MaxTemperatureC = response.Daily.MaxTemperature.ElementAtOrDefault(i),
                    Condition = GetOpenMeteoCondition(code),
                    IconUrl = null
                });
            }

            return new WeatherResultViewModel
            {
                LocationName = location.Name,
                TemperatureC = response.Current.Temperature,
                Condition = GetOpenMeteoCondition(response.Current.WeatherCode),
                Humidity = response.Current.Humidity,
                WindKph = response.Current.WindSpeed,
                ForecastDays = forecastDays
            };
        }

        private async Task<OpenMeteoLocation> ResolveOpenMeteoLocationAsync(string query)
        {
            if (TryParseCoordinates(query, out var latitude, out var longitude))
            {
                return new OpenMeteoLocation
                {
                    Name = $"{latitude.ToString("0.####", CultureInfo.InvariantCulture)}, {longitude.ToString("0.####", CultureInfo.InvariantCulture)}",
                    Latitude = latitude,
                    Longitude = longitude
                };
            }

            var geocodingUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(query)}&count=1&language=vi&format=json";
            var response = await _httpClient.GetFromJsonAsync<OpenMeteoGeocodingResponse>(geocodingUrl);
            var result = response?.Results?.FirstOrDefault();
            if (result is null)
            {
                throw new InvalidOperationException("Không tìm thấy địa điểm.");
            }

            return new OpenMeteoLocation
            {
                Name = string.Join(", ", new[] { result.Name, result.Admin1, result.Country }.Where(value => !string.IsNullOrWhiteSpace(value))),
                Latitude = result.Latitude,
                Longitude = result.Longitude
            };
        }

        private static bool TryParseCoordinates(string query, out double latitude, out double longitude)
        {
            latitude = 0;
            longitude = 0;

            var parts = query.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 2
                && double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out latitude)
                && double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out longitude)
                && latitude is >= -90 and <= 90
                && longitude is >= -180 and <= 180;
        }

        private static string GetOpenMeteoCondition(int code)
        {
            return code switch
            {
                0 => "Trời quang",
                1 or 2 or 3 => "Có mây",
                45 or 48 => "Sương mù",
                51 or 53 or 55 => "Mưa phùn",
                56 or 57 => "Mưa phùn đóng băng",
                61 or 63 or 65 => "Mưa",
                66 or 67 => "Mưa đóng băng",
                71 or 73 or 75 => "Tuyết rơi",
                77 => "Hạt tuyết",
                80 or 81 or 82 => "Mưa rào",
                85 or 86 => "Tuyết rào",
                95 => "Giông bão",
                96 or 99 => "Giông bão kèm mưa đá",
                _ => "Không rõ"
            };
        }

        private static string? NormalizeIconUrl(string? iconUrl)
        {
            if (string.IsNullOrWhiteSpace(iconUrl))
            {
                return null;
            }

            return iconUrl.StartsWith("//", StringComparison.Ordinal) ? $"https:{iconUrl}" : iconUrl;
        }

        private class WeatherApiResponse
        {
            public LocationDto? Location { get; set; }

            public CurrentDto? Current { get; set; }

            public ForecastDto? Forecast { get; set; }
        }

        private class LocationDto
        {
            public string Name { get; set; } = string.Empty;

            public string Country { get; set; } = string.Empty;
        }

        private class CurrentDto
        {
            [JsonPropertyName("temp_c")]
            public double TempC { get; set; }

            public ConditionDto? Condition { get; set; }

            public int Humidity { get; set; }

            [JsonPropertyName("wind_kph")]
            public double WindKph { get; set; }
        }

        private class ForecastDto
        {
            [JsonPropertyName("forecastday")]
            public List<ForecastDayDto> ForecastDay { get; set; } = new();
        }

        private class ForecastDayDto
        {
            public string Date { get; set; } = string.Empty;

            public DayDto Day { get; set; } = new();
        }

        private class DayDto
        {
            [JsonPropertyName("mintemp_c")]
            public double MinTempC { get; set; }

            [JsonPropertyName("maxtemp_c")]
            public double MaxTempC { get; set; }

            public ConditionDto? Condition { get; set; }
        }

        private class ConditionDto
        {
            public string Text { get; set; } = string.Empty;

            public string Icon { get; set; } = string.Empty;
        }

        private class OpenMeteoLocation
        {
            public string Name { get; set; } = string.Empty;

            public double Latitude { get; set; }

            public double Longitude { get; set; }
        }

        private class OpenMeteoGeocodingResponse
        {
            public List<OpenMeteoGeocodingResult>? Results { get; set; }
        }

        private class OpenMeteoGeocodingResult
        {
            public string Name { get; set; } = string.Empty;

            public string? Admin1 { get; set; }

            public string? Country { get; set; }

            public double Latitude { get; set; }

            public double Longitude { get; set; }
        }

        private class OpenMeteoForecastResponse
        {
            public OpenMeteoCurrent? Current { get; set; }

            public OpenMeteoDaily? Daily { get; set; }
        }

        private class OpenMeteoCurrent
        {
            [JsonPropertyName("temperature_2m")]
            public double Temperature { get; set; }

            [JsonPropertyName("relative_humidity_2m")]
            public int Humidity { get; set; }

            [JsonPropertyName("wind_speed_10m")]
            public double WindSpeed { get; set; }

            [JsonPropertyName("weather_code")]
            public int WeatherCode { get; set; }
        }

        private class OpenMeteoDaily
        {
            public List<string> Time { get; set; } = new();

            [JsonPropertyName("weather_code")]
            public List<int> WeatherCode { get; set; } = new();

            [JsonPropertyName("temperature_2m_max")]
            public List<double> MaxTemperature { get; set; } = new();

            [JsonPropertyName("temperature_2m_min")]
            public List<double> MinTemperature { get; set; } = new();
        }
    }
}
