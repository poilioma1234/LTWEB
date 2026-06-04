using System.Diagnostics;
using b6.Models;
using b6.Services;
using Microsoft.AspNetCore.Mvc;

namespace b6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherService _weatherService;

        public HomeController(ILogger<HomeController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        public IActionResult Index()
        {
            return View(new WeatherViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WeatherViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                model.Result = await _weatherService.GetForecastAsync(model.Query!);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Khong the lay du lieu thoi tiet cho {Query}", model.Query);
                model.ErrorMessage = "Không tìm thấy dữ liệu thời tiết. Hãy thử tên thành phố khác hoặc nhập tọa độ theo dạng 21.0285,105.8542.";
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
