using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSys.API.Models
{
    public class BorrowRecord
    {
        [Key]
        public int BorrowId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

        public DateTime? ReturnDate { get; set; } // Null until returned

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Borrowed";
        // "Borrowed" or "Returned"

        // Navigation Properties
        [ForeignKey("BookId")]
        public Book Book { get; set; } = null!;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}