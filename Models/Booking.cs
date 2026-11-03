using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSCreationnProject.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Client name is required")]
        [Display(Name = "Full Name")]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Display(Name = "Contact Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select an event category")]
        [Display(Name = "Event Category")]
        public string EventType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; } = DateTime.Today.AddDays(7);

        [Required(ErrorMessage = "Please provide expected guest count")]
        [Range(20, 5000, ErrorMessage = "Guest count must be between 20 and 5000")]
        [Display(Name = "Expected Guests")]
        public int GuestCount { get; set; }

        [Required(ErrorMessage = "Estimated budget is required")]
        [Range(15000, 5000000, ErrorMessage = "Budget must be between ₹15,000 and ₹50,00,000")]
        [Display(Name = "Budget (INR)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedBudget { get; set; }

        [Required(ErrorMessage = "Venue or city location is required")]
        [Display(Name = "Venue / City")]
        public string VenueLocation { get; set; } = string.Empty;

        [Display(Name = "Special Themes & Requests")]
        public string? SpecialRequests { get; set; }
    }
}