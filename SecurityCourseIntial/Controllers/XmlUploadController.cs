using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace SecurityCourseIntial.Controllers
{
    public class XmlUploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadSVG(IFormFile svgFile)
        {
            var extension = Path.GetExtension(svgFile.FileName);

            if (extension != ".svg")
            {
                return BadRequest("Please upload an SVG file");
            }
            try
            {
                var tempFilePath = Path.GetTempFileName();
                Console.WriteLine(tempFilePath);
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await svgFile.CopyToAsync(stream);
                }

                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Parse, // Ensure that DTD is processed
                    XmlResolver = new XmlUrlResolver()    // Allow external entity resolution (insecure)
                };

                using (var reader = XmlReader.Create(tempFilePath, settings))
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(reader);  // Load the XML with the DTD processed

                    var resolvedSvgContent = xmlDoc.InnerXml;
                    ViewBag.SVGContent = resolvedSvgContent;  // Pass the resolved content to the view

                    Console.WriteLine(resolvedSvgContent);
                }

                System.IO.File.Delete(tempFilePath);

                return View("DisplaySVG");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong { ex.Message }");
            }
            return RedirectToAction("Index");
        }
    }
}
