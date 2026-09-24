using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
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
        public IActionResult GetAll()
        {
            // su dung reposity pattern
            var allBooks = _bookRepository.GetAllBooks();
            return Ok(allBooks);
        }
        [HttpGet]
        [Route("get-book-by-id/{id}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var bookWithIdDTO = _bookRepository.GetBookById(id);
            return Ok(bookWithIdDTO);
        }
        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] addBookRequestDTO addBookRequestDTO)
        {
            var bookAdd = _bookRepository.AddBook(addBookRequestDTO);
            return Ok(bookAdd);
        }
        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] addBookRequestDTO bookDTO)
        {
            var updateBook = _bookRepository.UpdateBook(id, bookDTO);
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
    }
}