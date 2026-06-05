using System.ComponentModel.DataAnnotations;

namespace b6.Models
{
    public class WeatherViewModel
    {
        [Display(Name = "Tên thành phố hoặc tọa độ")]
        [Required(ErrorMessage = "Vui lòng nhập tên thành phố hoặc tọa độ.")]
        public string? Query { get; set; }

        public WeatherResultViewModel? Result { get; set; }

        public string? ErrorMessage { get; set; }
    }

    public class WeatherResultViewModel
    {
        public string LocationName { get; set; } = string.Empty;

        public double TemperatureC { get; set; }

        public string Condition { get; set; } = string.Empty;

        public string? IconUrl { get; set; }

        public int Humidity { get; set; }

        public double WindKph { get; set; }

        public List<ForecastDayViewModel> ForecastDays { get; set; } = new();
    }

    public class ForecastDayViewModel
    {
        public DateOnly Date { get; set; }

        public double MinTemperatureC { get; set; }

        public double MaxTemperatureC { get; set; }

        public string Condition { get; set; } = string.Empty;

        public string? IconUrl { get; set; }
    }
}
