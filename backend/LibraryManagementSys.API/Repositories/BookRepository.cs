using LibraryManagementSys.API.Data;
using LibraryManagementSys.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSys.API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(AppDbContext context, ILogger<BookRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            try
            {
                return await _context.Books.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all books");
                throw;
            }
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            try
            {
                return await _context.Books
                    .FirstOrDefaultAsync(b => b.BookId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting book by id: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            try
            {
                return await _context.Books
                    .Where(b => b.Title.Contains(searchTerm) ||
                                b.Author.Contains(searchTerm))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching books: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Book added: {Title}", book.Title);
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book: {Title}", book.Title);
                throw;
            }
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            try
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Book updated: {BookId}", book.BookId);
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book: {BookId}", book.BookId);
                throw;
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null) return false;

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Book deleted: {BookId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book: {BookId}", id);
                throw;
            }
        }

        public async Task<bool> BookExistsAsync(int id)
        {
            try
            {
                return await _context.Books.AnyAsync(b => b.BookId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking book exists: {BookId}", id);
                throw;
            }
        }
    }
}