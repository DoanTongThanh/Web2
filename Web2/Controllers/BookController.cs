using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Web2.CustomActionFilter;
using Web2.Data;
using Web2.Models.Domain;
using Web2.Models.DTO;
using Web2.Repositories;

namespace Web2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IBookRepository _bookRepository;

        public BooksController(AppDbContext dbContext, IBookRepository bookRepository)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
        }

        [HttpGet("get-all-books")]
        public IActionResult GetAll(
    [FromQuery] string? filterOn,
    [FromQuery] string? filterQuery,
    [FromQuery] string? sortBy,
    [FromQuery] bool isAscending = true,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 100)
        {
            var allBooks = _bookRepository.GetAllBooks(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return Ok(allBooks);
        }

        [HttpGet]
        [Route("get-book-by-id/{id}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var bookWithIdDTO = _bookRepository.GetBookById(id);
            if (bookWithIdDTO == null)
            {
                return NotFound();
            }
            return Ok(bookWithIdDTO);
        }

        [HttpPost("add-book")]
        [ValidateModel]
        public IActionResult AddBook([FromBody] addBookRequestDTO addBookRequestDTO)
        {
            if (!ValidateAddBook(addBookRequestDTO))
            {
                return BadRequest(ModelState);
            }

            var bookAdd = _bookRepository.AddBook(addBookRequestDTO);
            return Ok(bookAdd);
        }

        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] addBookRequestDTO bookDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateBook = _bookRepository.UpdateBook(id, bookDTO);
            if (updateBook == null)
            {
                return NotFound();
            }
            return Ok(updateBook);
        }

        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var deleteBook = _bookRepository.DeleteBook(id);
            if (deleteBook == null)
            {
                return NotFound(new { message = "Không tìm thấy sách cần xóa" });
            }
            return Ok(new { message = "Đã xóa sách thành công!", deletedId = id });
        }

        #region Private methods
        private bool ValidateAddBook(addBookRequestDTO addBookRequestDTO)
        {
            if (addBookRequestDTO == null)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO), "Please add book data");
                return false;
            }

            
            if (string.IsNullOrEmpty(addBookRequestDTO.Description))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Description), $"{nameof(addBookRequestDTO.Description)} cannot be null");
            }

            
            if (addBookRequestDTO.Rate < 0 || addBookRequestDTO.Rate > 5)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Rate), $"{nameof(addBookRequestDTO.Rate)} cannot be less than 0 and more than 5");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}