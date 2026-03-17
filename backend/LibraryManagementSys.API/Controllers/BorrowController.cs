using LibraryManagementSys.API.DTOs;
using LibraryManagementSys.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace LibraryManagementSys.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;
        private readonly ILogger<BorrowController> _logger;

        public BorrowController(
            IBorrowService borrowService,
            ILogger<BorrowController> logger)
        {
            _borrowService = borrowService;
            _logger = logger;
        }

        // POST api/borrow
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BorrowBook(
            [FromBody] BorrowRequestDto borrowRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid borrow request model state");
                    return BadRequest(ModelState);
                }

                // Get logged in user id from token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userIdClaim == null)
                {
                    _logger.LogWarning("User id not found in token");
                    return Unauthorized(new { Message = "User not found in token" });
                }

                var userId = int.Parse(userIdClaim);

                _logger.LogInformation("Borrow request by user {UserId} for book {BookId}",
                    userId, borrowRequestDto.BookId);

                var result = await _borrowService
                    .BorrowBookAsync(userId, borrowRequestDto);

                return Ok(new
                {
                    Message = "Book borrowed successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error borrowing book");
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT api/borrow/return/{bookId}
        [HttpPut("return/{bookId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ReturnBook(int bookId)
        {
            try
            {
                // Get logged in user id from token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userIdClaim == null)
                {
                    _logger.LogWarning("User id not found in token");
                    return Unauthorized(new { Message = "User not found in token" });
                }

                var userId = int.Parse(userIdClaim);

                _logger.LogInformation("Return request by user {UserId} for book {BookId}",
                    userId, bookId);

                var result = await _borrowService.ReturnBookAsync(userId, bookId);

                return Ok(new
                {
                    Message = "Book returned successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning book");
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET api/borrow/history
        [HttpGet("history")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyBorrowHistory()
        {
            try
            {
                // Get logged in user id from token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userIdClaim == null)
                {
                    _logger.LogWarning("User id not found in token");
                    return Unauthorized(new { Message = "User not found in token" });
                }

                var userId = int.Parse(userIdClaim);

                _logger.LogInformation("Borrow history request for user: {UserId}",
                    userId);

                var result = await _borrowService.GetUserBorrowHistoryAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting borrow history");
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        // GET api/borrow/all
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBorrowRecords()
        {
            try
            {
                _logger.LogInformation("Get all borrow records request by Admin");

                var result = await _borrowService.GetAllBorrowRecordsAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all borrow records");
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }
    }
}
