using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSCreationnProject.Models;

namespace SSCreationnProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch top 6 recent events with their image collections
            var featuredEvents = await _context.EventGalleries
                                              .Include(e => e.GalleryImages)
                                              .OrderByDescending(e => e.EventDate)
                                              .Take(6)
                                              .ToListAsync();

            return View(featuredEvents);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}