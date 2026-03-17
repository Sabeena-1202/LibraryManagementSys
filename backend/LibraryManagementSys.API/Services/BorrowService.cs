using LibraryManagementSys.API.DTOs;
using LibraryManagementSys.API.Models;
using LibraryManagementSys.API.Repositories;

namespace LibraryManagementSys.API.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BorrowService> _logger;

        public BorrowService(
            IBorrowRepository borrowRepository,
            IBookRepository bookRepository,
            ILogger<BorrowService> logger)
        {
            _borrowRepository = borrowRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<BorrowResponseDto> BorrowBookAsync(
            int userId, BorrowRequestDto borrowRequestDto)
        {
            try
            {
                _logger.LogInformation("User {UserId} attempting to borrow book {BookId}",
                    userId, borrowRequestDto.BookId);

                // Check if book exists
                var book = await _bookRepository
                    .GetBookByIdAsync(borrowRequestDto.BookId);

                if (book == null)
                {
                    _logger.LogWarning("Book not found: {BookId}", borrowRequestDto.BookId);
                    throw new Exception("Book not found");
                }

                // Check if book is available
                if (book.AvailabilityStatus != "Available")
                {
                    _logger.LogWarning("Book not available: {BookId}",
                        borrowRequestDto.BookId);
                    throw new Exception("Book is not available for borrowing");
                }

                // Check if user already borrowed this book
                var existingRecord = await _borrowRepository
                    .GetActiveBorrowRecordAsync(borrowRequestDto.BookId, userId);

                if (existingRecord != null)
                {
                    _logger.LogWarning("User {UserId} already borrowed book {BookId}",
                        userId, borrowRequestDto.BookId);
                    throw new Exception("You have already borrowed this book");
                }

                // Create borrow record
                var borrowRecord = new BorrowRecord
                {
                    BookId = borrowRequestDto.BookId,
                    UserId = userId,
                    BorrowDate = DateTime.UtcNow,
                    Status = "Borrowed"
                };

                var createdRecord = await _borrowRepository
                    .CreateBorrowRecordAsync(borrowRecord);

                // Update book availability
                book.AvailabilityStatus = "Borrowed";
                await _bookRepository.UpdateBookAsync(book);

                _logger.LogInformation(
                    "Book {BookId} borrowed successfully by user {UserId}",
                    borrowRequestDto.BookId, userId);

                return new BorrowResponseDto
                {
                    BorrowId = createdRecord.BorrowId,
                    BookId = createdRecord.BookId,
                    BookTitle = book.Title,
                    UserName = createdRecord.User?.Name ?? string.Empty,
                    BorrowDate = createdRecord.BorrowDate,
                    ReturnDate = createdRecord.ReturnDate,
                    Status = createdRecord.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error borrowing book {BookId} for user {UserId}",
                    borrowRequestDto.BookId, userId);
                throw;
            }
        }

        public async Task<BorrowResponseDto> ReturnBookAsync(int userId, int bookId)
        {
            try
            {
                _logger.LogInformation("User {UserId} attempting to return book {BookId}",
                    userId, bookId);

                // Find active borrow record
                var borrowRecord = await _borrowRepository
                    .GetActiveBorrowRecordAsync(bookId, userId);

                if (borrowRecord == null)
                {
                    _logger.LogWarning(
                        "No active borrow record found for user {UserId} book {BookId}",
                        userId, bookId);
                    throw new Exception("No active borrow record found for this book");
                }

                // Update borrow record
                borrowRecord.ReturnDate = DateTime.UtcNow;
                borrowRecord.Status = "Returned";

                var updatedRecord = await _borrowRepository
                    .UpdateBorrowRecordAsync(borrowRecord);

                // Update book availability
                var book = await _bookRepository.GetBookByIdAsync(bookId);
                if (book != null)
                {
                    book.AvailabilityStatus = "Available";
                    await _bookRepository.UpdateBookAsync(book);
                }

                _logger.LogInformation(
                    "Book {BookId} returned successfully by user {UserId}",
                    bookId, userId);

                return new BorrowResponseDto
                {
                    BorrowId = updatedRecord.BorrowId,
                    BookId = updatedRecord.BookId,
                    BookTitle = book?.Title ?? string.Empty,
                    UserName = updatedRecord.User?.Name ?? string.Empty,
                    BorrowDate = updatedRecord.BorrowDate,
                    ReturnDate = updatedRecord.ReturnDate,
                    Status = updatedRecord.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning book {BookId} for user {UserId}",
                    bookId, userId);
                throw;
            }
        }

        public async Task<IEnumerable<BorrowResponseDto>> GetUserBorrowHistoryAsync(
            int userId)
        {
            try
            {
                _logger.LogInformation("Fetching borrow history for user: {UserId}",
                    userId);

                var records = await _borrowRepository
                    .GetBorrowRecordsByUserIdAsync(userId);

                return records.Select(r => new BorrowResponseDto
                {
                    BorrowId = r.BorrowId,
                    BookId = r.BookId,
                    BookTitle = r.Book?.Title ?? string.Empty,
                    UserName = r.User?.Name ?? string.Empty,
                    BorrowDate = r.BorrowDate,
                    ReturnDate = r.ReturnDate,
                    Status = r.Status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching borrow history for user: {UserId}",
                    userId);
                throw;
            }
        }

        public async Task<IEnumerable<BorrowResponseDto>> GetAllBorrowRecordsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all borrow records");

                var records = await _borrowRepository.GetAllBorrowRecordsAsync();

                return records.Select(r => new BorrowResponseDto
                {
                    BorrowId = r.BorrowId,
                    BookId = r.BookId,
                    BookTitle = r.Book?.Title ?? string.Empty,
                    UserName = r.User?.Name ?? string.Empty,
                    BorrowDate = r.BorrowDate,
                    ReturnDate = r.ReturnDate,
                    Status = r.Status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all borrow records");
                throw;
            }
        }
    }
}
