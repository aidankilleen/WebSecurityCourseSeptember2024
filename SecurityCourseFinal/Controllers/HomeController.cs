using Microsoft.AspNetCore.Mvc;
using SecurityCourseFinal.Models;
using System.Diagnostics;

namespace SecurityCourseFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, 
                              IWebHostEnvironment webHostEnvironment, 
                              IConfiguration configuration)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;

            Console.WriteLine(_webHostEnvironment.EnvironmentName);
        }

        public IActionResult Index()
        {
            var customSettings = _configuration.GetSection("CustomSettings");
            var applicationName = customSettings.GetValue<string>("ApplicationName");

            ViewBag.ApplicationName = applicationName;
            return View();
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
