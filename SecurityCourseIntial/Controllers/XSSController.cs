using Microsoft.AspNetCore.Mvc;

namespace SecurityCourseIntial.Controllers
{
    public class XSSController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userComment)
        {
            ViewBag.UserComment = userComment;

            return View();
        }
    }
}
