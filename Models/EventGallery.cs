using System.ComponentModel.DataAnnotations;

namespace SSCreationnProject.Models
{
    public class EventGallery
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Event title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty; // Wedding, Corporate, Birthday

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; } = DateTime.Today;

        public string? Description { get; set; }

        // Primary cover image shown on cards
        public string? ImageUrl { get; set; }

        // One event has many photos
        public List<EventImage> GalleryImages { get; set; } = new();
    }

    public class EventImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int EventGalleryId { get; set; }
        public EventGallery? EventGallery { get; set; }
    }
}