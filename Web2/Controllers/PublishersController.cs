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
    public class PublishersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IPublisherRepository _publisherRepository;

        public PublishersController(AppDbContext dbContext, IPublisherRepository publisherRepository)
        {
            _dbContext = dbContext;
            _publisherRepository = publisherRepository;
        }

        [HttpGet]
        [Route("get-all-publishers")]
        public IActionResult GetAllPublishers(
    [FromQuery] string? filterOn,
    [FromQuery] string? filterQuery,
    [FromQuery] string? sortBy,
    [FromQuery] bool? isAscending,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 1000)
        {
            var publishers = _publisherRepository.GetAllPublishers(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            return Ok(publishers);
        }

        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var publisherWithId = _publisherRepository.GetPublisherById(id);
            return Ok(publisherWithId);
        }

        [HttpPost("add-publisher")]
        [ValidateModel]
        public IActionResult AddPublisher([FromBody] AddPublisherRequestDTO addPublisherRequestDTO)
        {
            

            var publisherAdd = _publisherRepository.AddPublisher(addPublisherRequestDTO);
            return Ok(publisherAdd);
        }

        [HttpPut("update-publisher-by-id/{id}")]
        public IActionResult UpdatePublisherById(int id, [FromBody] PublisherNoIdDTO publisherDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisherUpdate = _publisherRepository.UpdatePublisherById(id, publisherDTO);
            return Ok(publisherUpdate);
        }

        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var publisherDelete = _publisherRepository.DeletePublisherById(id);
            return Ok();
        }
    }
}