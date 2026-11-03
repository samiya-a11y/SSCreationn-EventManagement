namespace SSCreationnProject.Models
{
    public class OwnerDashboardViewModel
    {
        public List<Booking> Bookings { get; set; } = new();
        public List<EventGallery> Events { get; set; } = new();
    }
}