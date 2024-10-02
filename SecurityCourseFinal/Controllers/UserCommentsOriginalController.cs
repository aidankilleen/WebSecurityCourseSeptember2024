using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityCourseFinal.Data;

namespace SecurityCourseFinal.Controllers
{

    public class UserCommentsOriginalController : Controller
    {

        private readonly ApplicationDbContext _dbcontext;

        public UserCommentsOriginalController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var comments = await _dbcontext.UserComments.ToListAsync();

            return View(comments);
        }
    }
}
