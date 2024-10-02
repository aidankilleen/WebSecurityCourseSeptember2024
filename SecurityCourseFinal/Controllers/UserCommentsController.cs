using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecurityCourseFinal.Data;
using SecurityCourseFinal.Models;

namespace SecurityCourseFinal.Controllers
{
    public class UserCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserComments
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserComments.ToListAsync());
        }

        // GET: UserComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userComment = await _context.UserComments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userComment == null)
            {
                return NotFound();
            }

            return View(userComment);
        }

        // GET: UserComments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Author,Content,CreatedAt")] UserComment userComment)
        {
            if (ModelState.IsValid)
            {
                // sanitize 
                var sanitizer = new HtmlSanitizer();

                sanitizer.AllowedTags.Clear();

                userComment.Author = sanitizer.Sanitize(userComment.Author);
                userComment.Content = sanitizer.Sanitize(userComment.Content);

                if (userComment.Author.Length == 0 || userComment.Content.Length == 0)
                {
                    return BadRequest("Comment empty or invalid");
                }

                _context.Add(userComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userComment);
        }

        // GET: UserComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userComment = await _context.UserComments.FindAsync(id);
            if (userComment == null)
            {
                return NotFound();
            }
            return View(userComment);
        }

        // POST: UserComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Author,Content,CreatedAt")] UserComment userComment)
        {
            if (id != userComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCommentExists(userComment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userComment);
        }

        // GET: UserComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userComment = await _context.UserComments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userComment == null)
            {
                return NotFound();
            }

            return View(userComment);
        }

        // POST: UserComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userComment = await _context.UserComments.FindAsync(id);
            if (userComment != null)
            {
                _context.UserComments.Remove(userComment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCommentExists(int id)
        {
            return _context.UserComments.Any(e => e.Id == id);
        }
    }
}
