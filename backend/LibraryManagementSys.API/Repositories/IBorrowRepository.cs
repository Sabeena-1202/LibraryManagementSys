using LibraryManagementSys.API.Models;

namespace LibraryManagementSys.API.Repositories
{
    public interface IBorrowRepository
    {
        Task<IEnumerable<BorrowRecord>> GetAllBorrowRecordsAsync();
        Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByUserIdAsync(int userId);
        Task<BorrowRecord?> GetActiveBorrowRecordAsync(int bookId, int userId);
        Task<BorrowRecord> CreateBorrowRecordAsync(BorrowRecord record);
        Task<BorrowRecord> UpdateBorrowRecordAsync(BorrowRecord record);
    }
}