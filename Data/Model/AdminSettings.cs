using System.ComponentModel.DataAnnotations;

namespace Ali_Store.Data.Model
{
    public class AdminSettings
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Default Order Cancellation Hours")]
        [Range(1, 168, ErrorMessage = "Value must be between 1 and 168 hours (7 days)")]
        public int DefaultCancellationHours { get; set; } = 24;
    }
} 