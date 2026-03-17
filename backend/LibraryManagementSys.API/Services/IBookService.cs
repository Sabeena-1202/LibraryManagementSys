using LibraryManagementSys.API.DTOs;

namespace LibraryManagementSys.API.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponseDto>> GetAllBooksAsync();
        Task<BookResponseDto?> GetBookByIdAsync(int id);
        Task<IEnumerable<BookResponseDto>> SearchBooksAsync(string searchTerm);
        Task<BookResponseDto> AddBookAsync(BookCreateDto bookCreateDto);
        Task<BookResponseDto> UpdateBookAsync(int id, BookUpdateDto bookUpdateDto);
        Task<bool> DeleteBookAsync(int id);
    }
}