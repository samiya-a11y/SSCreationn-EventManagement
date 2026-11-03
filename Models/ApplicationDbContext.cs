using Microsoft.EntityFrameworkCore;

namespace SSCreationnProject.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EventGallery> EventGalleries { get; set; }
        public DbSet<EventImage> EventImages { get; set; }
    }
}