using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSys.API.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        public int PublishedYear { get; set; }

        [Required]
        [MaxLength(20)]
        public string AvailabilityStatus { get; set; } = "Available";
        // "Available" or "Borrowed"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public ICollection<BorrowRecord> BorrowRecords { get; set; }
            = new List<BorrowRecord>();
    }
}