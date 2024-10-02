using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecurityCourseFinal.Data;
using SecurityCourseFinal.Models;

namespace SecurityCourseFinal.Controllers
{
    public class UserImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UserImagesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: UserImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserImages.ToListAsync());
        }

        // GET: UserImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userImage = await _context.UserImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userImage == null)
            {
                return NotFound();
            }

            return View(userImage);
        }

        // GET: UserImages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Path")] UserImage userImage, IFormFile ImageUpload)
        {
            var extension = Path.GetExtension(ImageUpload.FileName);

            var folder = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var filename = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
            var savePath = Path.Combine(folder, filename + extension);
            userImage.Path = Path.Combine("/uploads", filename + extension);


            // check this is an image

            // check the extension
            var allowedExtensions = new[] { ".jpg", ".gif", ".png" };

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Invalid file format");
            }

            bool validFile = false;
            // check that this really is an image
            using (var reader = new BinaryReader(ImageUpload.OpenReadStream()))
            {
                // Read the file signature (the first 8 bytes should be enough for most image formats)
                var signatures = new Dictionary<string, List<byte[]>>
                {
                    { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },              // PNG
                    { ".jpg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } } },                                             // JPEG
                    { ".jpeg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } } },                                            // JPEG
                    { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } }                                        // GIF
                };

                // Read the file header
                var headerBytes = reader.ReadBytes(8);  // Read first 8 bytes

                // Check the signature against known patterns for the given file extension
                if (signatures.ContainsKey(extension))
                {
                    foreach (var signature in signatures[extension])
                    {
                        if (headerBytes.Take(signature.Length).SequenceEqual(signature))
                        {
                            validFile = true;
                        }
                    }
                }
            }
            if (!validFile)
            {
                return BadRequest("file signature not matched");
            } 




            if (ModelState.IsValid)
            {
                // save the uploaded file here
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(fileStream);
                }

                

                _context.Add(userImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userImage);
        }

        // GET: UserImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userImage = await _context.UserImages.FindAsync(id);
            if (userImage == null)
            {
                return NotFound();
            }
            return View(userImage);
        }

        // POST: UserImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Path")] UserImage userImage)
        {
            if (id != userImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserImageExists(userImage.Id))
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
            return View(userImage);
        }

        // GET: UserImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userImage = await _context.UserImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userImage == null)
            {
                return NotFound();
            }

            return View(userImage);
        }

        // POST: UserImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userImage = await _context.UserImages.FindAsync(id);
            if (userImage != null)
            {
                _context.UserImages.Remove(userImage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserImageExists(int id)
        {
            return _context.UserImages.Any(e => e.Id == id);
        }
    }
}
