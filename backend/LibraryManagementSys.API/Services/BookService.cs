using LibraryManagementSys.API.DTOs;
using LibraryManagementSys.API.Models;
using LibraryManagementSys.API.Repositories;

namespace LibraryManagementSys.API.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookService> _logger;

        public BookService(
            IBookRepository bookRepository,
            ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all books");

                var books = await _bookRepository.GetAllBooksAsync();

                return books.Select(b => new BookResponseDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Category = b.Category,
                    PublishedYear = b.PublishedYear,
                    AvailabilityStatus = b.AvailabilityStatus
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all books");
                throw;
            }
        }

        public async Task<BookResponseDto?> GetBookByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching book by id: {BookId}", id);

                var book = await _bookRepository.GetBookByIdAsync(id);

                if (book == null)
                {
                    _logger.LogWarning("Book not found: {BookId}", id);
                    return null;
                }

                return new BookResponseDto
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    Category = book.Category,
                    PublishedYear = book.PublishedYear,
                    AvailabilityStatus = book.AvailabilityStatus
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching book by id: {BookId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<BookResponseDto>> SearchBooksAsync(string searchTerm)
        {
            try
            {
                _logger.LogInformation("Searching books with term: {SearchTerm}",
                    searchTerm);

                var books = await _bookRepository.SearchBooksAsync(searchTerm);

                return books.Select(b => new BookResponseDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Category = b.Category,
                    PublishedYear = b.PublishedYear,
                    AvailabilityStatus = b.AvailabilityStatus
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching books: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<BookResponseDto> AddBookAsync(BookCreateDto bookCreateDto)
        {
            try
            {
                _logger.LogInformation("Adding new book: {Title}", bookCreateDto.Title);

                var book = new Book
                {
                    Title = bookCreateDto.Title,
                    Author = bookCreateDto.Author,
                    Category = bookCreateDto.Category,
                    PublishedYear = bookCreateDto.PublishedYear,
                    AvailabilityStatus = "Available",
                    CreatedAt = DateTime.UtcNow
                };

                var createdBook = await _bookRepository.AddBookAsync(book);

                _logger.LogInformation("Book added successfully: {BookId}",
                    createdBook.BookId);

                return new BookResponseDto
                {
                    BookId = createdBook.BookId,
                    Title = createdBook.Title,
                    Author = createdBook.Author,
                    Category = createdBook.Category,
                    PublishedYear = createdBook.PublishedYear,
                    AvailabilityStatus = createdBook.AvailabilityStatus
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book: {Title}", bookCreateDto.Title);
                throw;
            }
        }

        public async Task<BookResponseDto> UpdateBookAsync(int id, BookUpdateDto bookUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating book: {BookId}", id);

                var book = await _bookRepository.GetBookByIdAsync(id);

                if (book == null)
                {
                    _logger.LogWarning("Book not found for update: {BookId}", id);
                    throw new Exception($"Book with id {id} not found");
                }

                // Only update fields that are provided
                if (bookUpdateDto.Title != null)
                    book.Title = bookUpdateDto.Title;
                if (bookUpdateDto.Author != null)
                    book.Author = bookUpdateDto.Author;
                if (bookUpdateDto.Category != null)
                    book.Category = bookUpdateDto.Category;
                if (bookUpdateDto.PublishedYear.HasValue)
                    book.PublishedYear = bookUpdateDto.PublishedYear.Value;
                if (bookUpdateDto.AvailabilityStatus != null)
                    book.AvailabilityStatus = bookUpdateDto.AvailabilityStatus;

                var updatedBook = await _bookRepository.UpdateBookAsync(book);

                _logger.LogInformation("Book updated successfully: {BookId}", id);

                return new BookResponseDto
                {
                    BookId = updatedBook.BookId,
                    Title = updatedBook.Title,
                    Author = updatedBook.Author,
                    Category = updatedBook.Category,
                    PublishedYear = updatedBook.PublishedYear,
                    AvailabilityStatus = updatedBook.AvailabilityStatus
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book: {BookId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting book: {BookId}", id);

                var exists = await _bookRepository.BookExistsAsync(id);

                if (!exists)
                {
                    _logger.LogWarning("Book not found for delete: {BookId}", id);
                    throw new Exception($"Book with id {id} not found");
                }

                var result = await _bookRepository.DeleteBookAsync(id);

                _logger.LogInformation("Book deleted successfully: {BookId}", id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book: {BookId}", id);
                throw;
            }
        }
    }
}