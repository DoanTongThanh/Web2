using Web2.Models.Domain;
using Web2.Models.DTO;

namespace Web2.Repositories
{
    public interface IPublisherRepository
    {
        List<PublisherDTO> GetAllPublishers(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        PublisherNoIdDTO GetPublisherById(int id);
        AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO publisherDto);
        PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO publisherDto);
        Publishers? DeletePublisherById(int id);
    }
}
