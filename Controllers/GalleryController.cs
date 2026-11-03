using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSCreationnProject.Models;

namespace SSCreationnProject.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public GalleryController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /Gallery
        public async Task<IActionResult> Index(string? category)
        {
            var query = _context.EventGalleries
                                .Include(e => e.GalleryImages)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                query = query.Where(e => e.Category == category);
            }

            ViewBag.SelectedCategory = category ?? "All";
            var events = await query.OrderByDescending(e => e.EventDate).ToListAsync();
            return View(events);
        }

        // GET: /Gallery/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("IsOwner") != "true")
            {
                return RedirectToAction("Login", "Booking");
            }
            return View();
        }

        // POST: /Gallery/Create (Uploads multiple images)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventGallery model, List<IFormFile>? imageFiles)
        {
            if (HttpContext.Session.GetString("IsOwner") != "true")
            {
                return RedirectToAction("Login", "Booking");
            }

            if (ModelState.IsValid)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "events");
                Directory.CreateDirectory(uploadsFolder);

                if (imageFiles != null && imageFiles.Count > 0)
                {
                    for (int i = 0; i < imageFiles.Count; i++)
                    {
                        var file = imageFiles[i];
                        if (file.Length > 0)
                        {
                            string uniqueName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                            string filePath = Path.Combine(uploadsFolder, uniqueName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            string relativePath = "/uploads/events/" + uniqueName;

                            // First image is the primary cover
                            if (i == 0)
                            {
                                model.ImageUrl = relativePath;
                            }

                            model.GalleryImages.Add(new EventImage { ImageUrl = relativePath });
                        }
                    }
                }

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Booking");
            }

            return View(model);
        }

        // POST: /Gallery/Edit/5 (Update Details + Add more photos)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventGallery model, List<IFormFile>? newImageFiles)
        {
            if (HttpContext.Session.GetString("IsOwner") != "true")
            {
                return RedirectToAction("Login", "Booking");
            }

            if (id != model.Id) return NotFound();

            var existingEvent = await _context.EventGalleries
                                              .Include(e => e.GalleryImages)
                                              .FirstOrDefaultAsync(e => e.Id == id);

            if (existingEvent == null) return NotFound();

            existingEvent.Title = model.Title;
            existingEvent.Category = model.Category;
            existingEvent.Location = model.Location;
            existingEvent.EventDate = model.EventDate;
            existingEvent.Description = model.Description;

            if (newImageFiles != null && newImageFiles.Count > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "events");
                Directory.CreateDirectory(uploadsFolder);

                foreach (var file in newImageFiles)
                {
                    if (file.Length > 0)
                    {
                        string uniqueName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        string relativePath = "/uploads/events/" + uniqueName;

                        if (string.IsNullOrEmpty(existingEvent.ImageUrl))
                        {
                            existingEvent.ImageUrl = relativePath;
                        }

                        existingEvent.GalleryImages.Add(new EventImage { ImageUrl = relativePath });
                    }
                }
            }

            _context.Update(existingEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Booking");
        }

        // POST: /Gallery/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("IsOwner") != "true")
            {
                return RedirectToAction("Login", "Booking");
            }

            var item = await _context.EventGalleries
                                     .Include(e => e.GalleryImages)
                                     .FirstOrDefaultAsync(e => e.Id == id);

            if (item != null)
            {
                foreach (var img in item.GalleryImages)
                {
                    if (!string.IsNullOrEmpty(img.ImageUrl) && img.ImageUrl.StartsWith("/uploads/"))
                    {
                        string fullPath = Path.Combine(_environment.WebRootPath, img.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                    }
                }

                _context.EventGalleries.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Booking");
        }
    }
}