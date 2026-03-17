using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSys.API.DTOs
{
    public class BorrowRequestDto
    {
        [Required]
        public int BookId { get; set; }
    }
}