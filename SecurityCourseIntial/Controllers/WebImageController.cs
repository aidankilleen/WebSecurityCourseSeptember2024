using Microsoft.AspNetCore.Mvc;

namespace SecurityCourseIntial.Controllers
{
    public class WebImageController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public WebImageController(IWebHostEnvironment webHostEnvironment, 
                                  IHttpClientFactory httpClientFactory)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FetchImage(string imageUrl)
        {

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(imageUrl);

            if (response.IsSuccessStatusCode)
            {
                var imageBytes = await response.Content.ReadAsByteArrayAsync();

                var imageBase64 = Convert.ToBase64String(imageBytes);

                ViewBag.ImageBase64 = imageBase64;

                return View("DisplayImage");
            }
            else
            {
                return BadRequest("Failed to fetch image");

            }
        }
    }
}
