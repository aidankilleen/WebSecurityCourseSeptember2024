using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecurityCourseFinal.Controllers
{
    [Authorize]
    public class MembersOnlyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
