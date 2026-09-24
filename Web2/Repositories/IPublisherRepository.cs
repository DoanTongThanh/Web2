using Web2.Models.Domain;
using Web2.Models.DTO;

namespace Web2.Repositories
{
    public interface IPublisherRepository
    {
        List<PublisherDTO> GetAllPublishers();
        PublisherNoIdDTO GetPublisherById(int id);
        AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO publisherDto);
        PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO publisherDto);
        Publishers? DeletePublisherById(int id);
    }
}
