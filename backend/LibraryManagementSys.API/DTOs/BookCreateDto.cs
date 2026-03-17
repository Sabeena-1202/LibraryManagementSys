using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSys.API.DTOs
{
    public class BookCreateDto
    {
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
    }
}