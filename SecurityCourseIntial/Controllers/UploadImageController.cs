using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace SecurityCourseIntial.Controllers
{
    public class UploadImageController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UploadImageController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            // get a list of the files in the upload folder and send them to the View

            var uploadFolder = $"{_webHostEnvironment.WebRootPath}\\uploads\\";
            var images = new List<string>();

            if (Directory.Exists(uploadFolder))
            {
                foreach (var file in Directory.GetFiles(uploadFolder))
                {
                    var fileName = Path.GetFileName(file);
                    images.Add("/uploads/" + fileName);
                }
            }

            return View(images);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file, string description)
        {

            var extension = Path.GetExtension(file.FileName);


            var permittedExtensions = new[] { ".jpg", ".png", ".gif", ".jfif" };

            if (permittedExtensions.Contains(extension))
            {
                var fileName = $"{description}{extension}";

                var sanitizedFilename = SanitizeFileName(fileName);

                Console.WriteLine(sanitizedFilename);

                // combining paths together using concatenation is insecure
                // almost the same as the SQL injection attack
                var filePath = $"{_webHostEnvironment.WebRootPath}\\uploads\\{ fileName }";

                // Solution - use Path.Combine instead

                //var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", sanitizedFilename);

                // create path
                Console.WriteLine(filePath);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return RedirectToAction("Index");
    
        }

        private string SanitizeFileName(string path)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitizedDescription = string.Join("_", path.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
            sanitizedDescription = Regex.Replace(sanitizedDescription, @"\s+", "_");
            return sanitizedDescription.ToLowerInvariant();
        }
    }
}
