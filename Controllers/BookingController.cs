using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSCreationnProject.Models;

namespace SSCreationnProject.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Single Owner Credentials
        private const string OWNER_USER = "admin";
        private const string OWNER_PASS = "admin@sscreationn";

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= CUSTOMER BOOKING FORM =================

        // GET: /Booking/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Confirmation), new { id = booking.Id });
            }
            return View(booking);
        }

        // GET: /Booking/Confirmation/5
        public async Task<IActionResult> Confirmation(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Create));
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        // ================= OWNER LOGIN & LOGOUT =================

        // GET: /Booking/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("IsOwner") == "true")
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // POST: /Booking/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (username == OWNER_USER && password == OWNER_PASS)
            {
                HttpContext.Session.SetString("IsOwner", "true");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "Invalid Owner credentials!";
            return View();
        }

        // GET: /Booking/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ================= OWNER DASHBOARD =================

        // GET: /Booking (Owner Dashboard)
        public async Task<IActionResult> Index(string? searchString)
        {
            if (HttpContext.Session.GetString("IsOwner") != "true")
            {
                return RedirectToAction(nameof(Login));
            }

            var bookingsQuery = _context.Bookings.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                bookingsQuery = bookingsQuery.Where(b => b.ClientName.Contains(searchString) || b.PhoneNumber.Contains(searchString));
            }

            var dashboardData = new OwnerDashboardViewModel
            {
                Bookings = await bookingsQuery.OrderByDescending(b => b.EventDate).ToListAsync(),
                Events = await _context.EventGalleries
                       .Include(e => e.GalleryImages)
                       .OrderByDescending(e => e.EventDate)
                       .ToListAsync()
            };

            return View(dashboardData);
        }

        // POST: /Booking/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("IsOwner") != "true")
            {
                return RedirectToAction(nameof(Login));
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}