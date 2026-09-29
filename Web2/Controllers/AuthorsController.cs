using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web2.CustomActionFilter;
using Web2.Data;
using Web2.Models.Domain;
using Web2.Models.DTO;
using Web2.Repositories;

namespace Web2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(AppDbContext dbContext, IAuthorRepository authorRepository)
        {
            _dbContext = dbContext;
            _authorRepository = authorRepository;
        }

        [HttpGet]
        [Route("get-all-authors")]
        public IActionResult GetAllAuthors(
    [FromQuery] string? filterOn,
    [FromQuery] string? filterQuery,
    [FromQuery] string? sortBy,
    [FromQuery] bool? isAscending,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 1000)
        {
            var authors = _authorRepository.GetAllAuthors(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            return Ok(authors);
        }

        [HttpGet("get-author-by-id/{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var authorWithId = _authorRepository.GetAuthorById(id);
            return Ok(authorWithId);
        }

        [HttpPost("add-author")]
        [ValidateModel]
        public IActionResult AddAuthors([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            

            var authorAdd = _authorRepository.AddAuthor(addAuthorRequestDTO);
            return Ok(authorAdd);
        }

        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] AuthorNoIdDTO authorNoIdDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorUpdate = _authorRepository.UpdateAuthorById(id, authorNoIdDTO);
            return Ok(authorUpdate);
        }

        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var authorDelete = _authorRepository.DeleteAuthorById(id);
            return Ok();
        }
    }
}