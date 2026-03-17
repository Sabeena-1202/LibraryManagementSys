using LibraryManagementSys.API.DTOs;
using LibraryManagementSys.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSys.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(
            IBookService bookService,
            ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        // GET api/book
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                _logger.LogInformation("Get all books request");
                var books = await _bookService.GetAllBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all books");
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        // GET api/book/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                _logger.LogInformation("Get book by id: {BookId}", id);
                var book = await _bookService.GetBookByIdAsync(id);

                if (book == null)
                    return NotFound(new { Message = $"Book with id {id} not found" });

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting book by id: {BookId}", id);
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        // GET api/book/search?searchTerm=harry
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchBooks([FromQuery] string searchTerm)
        {
            try
            {
                _logger.LogInformation("Search books: {SearchTerm}", searchTerm);

                if (string.IsNullOrWhiteSpace(searchTerm))
                    return BadRequest(new { Message = "Search term cannot be empty" });

                var books = await _bookService.SearchBooksAsync(searchTerm);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching books: {SearchTerm}", searchTerm);
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        // POST api/book
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBook([FromBody] BookCreateDto bookCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid book model state");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Add book request: {Title}", bookCreateDto.Title);
                var book = await _bookService.AddBookAsync(bookCreateDto);

                return CreatedAtAction(nameof(GetBookById),
                    new { id = book.BookId }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book: {Title}", bookCreateDto.Title);
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        // PUT api/book/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(
            int id, [FromBody] BookUpdateDto bookUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid book update model state");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Update book request: {BookId}", id);
                var book = await _bookService.UpdateBookAsync(id, bookUpdateDto);
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book: {BookId}", id);
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE api/book/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                _logger.LogInformation("Delete book request: {BookId}", id);
                var result = await _bookService.DeleteBookAsync(id);

                if (!result)
                    return NotFound(new { Message = $"Book with id {id} not found" });

                return Ok(new { Message = "Book deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book: {BookId}", id);
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}