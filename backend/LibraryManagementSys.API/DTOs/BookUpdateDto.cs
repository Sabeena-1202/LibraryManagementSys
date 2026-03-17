using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSys.API.DTOs
{
    public class BookUpdateDto
    {
        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(100)]
        public string? Author { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        public int? PublishedYear { get; set; }

        [MaxLength(20)]
        public string? AvailabilityStatus { get; set; }
    }
}