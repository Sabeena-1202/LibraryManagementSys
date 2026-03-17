using LibraryManagementSys.API.Data;
using LibraryManagementSys.API.Models;
using LibraryManagementSys.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSys.API.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BorrowRepository> _logger;

        public BorrowRepository(AppDbContext context, ILogger<BorrowRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<BorrowRecord>> GetAllBorrowRecordsAsync()
        {
            try
            {
                return await _context.BorrowRecords
                    .Include(b => b.Book)
                    .Include(b => b.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all borrow records");
                throw;
            }
        }

        public async Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByUserIdAsync(int userId)
        {
            try
            {
                return await _context.BorrowRecords
                    .Include(b => b.Book)
                    .Include(b => b.User)
                    .Where(b => b.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting borrow records for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<BorrowRecord?> GetActiveBorrowRecordAsync(int bookId, int userId)
        {
            try
            {
                return await _context.BorrowRecords
                    .FirstOrDefaultAsync(b => b.BookId == bookId &&
                                             b.UserId == userId &&
                                             b.Status == "Borrowed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active borrow record");
                throw;
            }
        }

        public async Task<BorrowRecord> CreateBorrowRecordAsync(BorrowRecord record)
        {
            try
            {
                _context.BorrowRecords.Add(record);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Borrow record created for BookId: {BookId}", record.BookId);
                return record;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating borrow record");
                throw;
            }
        }

        public async Task<BorrowRecord> UpdateBorrowRecordAsync(BorrowRecord record)
        {
            try
            {
                _context.BorrowRecords.Update(record);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Borrow record updated: {BorrowId}", record.BorrowId);
                return record;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating borrow record: {BorrowId}", record.BorrowId);
                throw;
            }
        }
    }
}
