using LibraryManagementSys.API.DTOs;

namespace LibraryManagementSys.API.Services
{
    public interface IBorrowService
    {
        Task<BorrowResponseDto> BorrowBookAsync(int userId, BorrowRequestDto borrowRequestDto);
        Task<BorrowResponseDto> ReturnBookAsync(int userId, int bookId);
        Task<IEnumerable<BorrowResponseDto>> GetUserBorrowHistoryAsync(int userId);
        Task<IEnumerable<BorrowResponseDto>> GetAllBorrowRecordsAsync();
    }
}