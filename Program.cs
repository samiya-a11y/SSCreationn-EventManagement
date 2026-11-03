using Microsoft.EntityFrameworkCore;
using SSCreationnProject.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Session Configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(45);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// SQLite Configuration
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "SSCreationn.db");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Auto-seed initial gallery records across all categories with multiple related photos
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();

    if (!db.EventGalleries.Any())
    {
        db.EventGalleries.AddRange(
            // ================= WEDDINGS & ENGAGEMENTS =================
            new EventGallery
            {
                Title = "Royal Heritage Palace Wedding",
                Category = "Wedding",
                Location = "Sayaji Lawns & Palace Grounds, Vadodara",
                EventDate = DateTime.Today.AddDays(-15),
                Description = "Grand crystal floral mandap, royal velvet aisle runner, and fairy-light entryway designed for 700+ guests.",
                ImageUrl = "https://images.unsplash.com/photo-1519741497674-611481863552?auto=format&fit=crop&w=900&q=80",
                GalleryImages = new List<EventImage>
                {
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1519741497674-611481863552?auto=format&fit=crop&w=900&q=80" },
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1511285560929-80b456fea0bc?auto=format&fit=crop&w=900&q=80" },
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1545232979-8bf68ee9b1af?auto=format&fit=crop&w=900&q=80" }
                }
            },
            new EventGallery
            {
                Title = "Bespoke Glasshouse Wedding Reception",
                Category = "Wedding",
                Location = "The Grand Courtyard, Ahmedabad",
                EventDate = DateTime.Today.AddDays(-40),
                Description = "Cascading chandelier installations, soft champagne draping, and customized couple stage architecture.",
                ImageUrl = "https://images.unsplash.com/photo-1511285560929-80b456fea0bc?auto=format&fit=crop&w=900&q=80",
                GalleryImages = new List<EventImage>
                {
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1511285560929-80b456fea0bc?auto=format&fit=crop&w=900&q=80" },
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1519741497674-611481863552?auto=format&fit=crop&w=900&q=80" }
                }
            },

            // ================= CORPORATE CONFERENCES & GALAS =================
            new EventGallery
            {
                Title = "Global Tech Leaders Annual Summit",
                Category = "Corporate",
                Location = "Grand Ballroom, Convention Centre",
                EventDate = DateTime.Today.AddDays(-28),
                Description = "Ultrawide LED projection walls, customized stage podiums, acoustic calibration, and keynote seating.",
                ImageUrl = "https://images.unsplash.com/photo-1511578314322-379afb476865?auto=format&fit=crop&w=900&q=80",
                GalleryImages = new List<EventImage>
                {
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1511578314322-379afb476865?auto=format&fit=crop&w=900&q=80" },
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1505373877841-8d25f7d46678?auto=format&fit=crop&w=900&q=80" },
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1475721027785-f74eccf877e2?auto=format&fit=crop&w=900&q=80" }
                }
            },
            new EventGallery
            {
                Title = "Executive Leadership Awards Gala",
                Category = "Corporate",
                Location = "Regency Sky Pavillion",
                EventDate = DateTime.Today.AddDays(-60),
                Description = "Black-tie dinner banquet, custom trophy display backdrops, and synchronized ambient illumination.",
                ImageUrl = "https://images.unsplash.com/photo-1505373877841-8d25f7d46678?auto=format&fit=crop&w=900&q=80",
                GalleryImages = new List<EventImage>
                {
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1505373877841-8d25f7d46678?auto=format&fit=crop&w=900&q=80" },
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1511578314322-379afb476865?auto=format&fit=crop&w=900&q=80" }
                }
            },

            // ================= BIRTHDAYS & PRIVATE CELEBRATIONS =================
            new EventGallery
            {
                Title = "Bohemian Meadow 25th Milestone Birthday",
                Category = "Birthday",
                Location = "Riverfront Open Air Lawn",
                EventDate = DateTime.Today.AddDays(-10),
                Description = "Pastel teepee lounges, dried floral arch installations, fairy string ceilings, and live acoustic music arrangement.",
                ImageUrl = "https://images.unsplash.com/photo-1464366400600-7168b8af9bc3?auto=format&fit=crop&w=900&q=80",
                GalleryImages = new List<EventImage>
                {
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1464366400600-7168b8af9bc3?auto=format&fit=crop&w=900&q=80" },
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1530103862676-de8c9debad1d?auto=format&fit=crop&w=900&q=80" },
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1527529482837-4698179dc6ce?auto=format&fit=crop&w=900&q=80" }
                }
            },
            new EventGallery
            {
                Title = "Golden Jubilee 50th Wedding Anniversary",
                Category = "Birthday",
                Location = "The Crystal Dining Hall",
                EventDate = DateTime.Today.AddDays(-55),
                Description = "Warm amber and gold accents, vintage candelabra tablescapes, and personalized photo memory gallery.",
                ImageUrl = "https://images.unsplash.com/photo-1530103862676-de8c9debad1d?auto=format&fit=crop&w=900&q=80",
                GalleryImages = new List<EventImage>
                {
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1530103862676-de8c9debad1d?auto=format&fit=crop&w=900&q=80" },
                    new EventImage { ImageUrl = "https://images.unsplash.com/photo-1464366400600-7168b8af9bc3?auto=format&fit=crop&w=900&q=80" }
                }
            }
        );
        db.SaveChanges();
    }
}

app.Run();