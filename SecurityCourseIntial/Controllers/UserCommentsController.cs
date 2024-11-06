using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SecurityCourseIntial.Models;
using SecurityCourseIntial.Repositories;
using Microsoft.Security.Application;
using Ganss.Xss;

namespace SecurityCourseIntial.Controllers
{
    public class UserCommentsController : Controller
    {
        private readonly UserCommentRepository _userCommentsRepository;
        public UserCommentsController(UserCommentRepository userCommentsRepository)
        {
            _userCommentsRepository = userCommentsRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userComments = await _userCommentsRepository.GetAll();
            Console.WriteLine(userComments);
            return View(userComments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserComment comment)
        {
            if (ModelState.IsValid)
            {
                //comment.Content = Encoder.HtmlEncode(comment.Content);

                //var sanitizer = new HtmlSanitizer();

                //var cleanContent = sanitizer.Sanitize(comment.Content);

                await _userCommentsRepository.Add(comment);
                return RedirectToAction("Index");


            }
            return View(comment);
        }
    }
}
