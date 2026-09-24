using Web2.Data;
using Web2.Models.Domain;
using Web2.Models.DTO;

namespace Web2.Repositories
{
    public class SQLPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLPublisherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<PublisherDTO> GetAllPublishers()
        {
            var publishersDomain = _dbContext.Publishers.ToList();
            var publisherDTOs = new List<PublisherDTO>();
            foreach (var pub in publishersDomain)
            {
                publisherDTOs.Add(new PublisherDTO()
                {
                    Id = pub.Id,
                    Name = pub.Name
                });
            }
            return publisherDTOs;
        }

        public PublisherNoIdDTO GetPublisherById(int id)
        {
            var publisherDomain = _dbContext.Publishers.FirstOrDefault(x => x.Id == id);
            if (publisherDomain == null)
            {
                return null!;
            }
            return new PublisherNoIdDTO
            {
                Name = publisherDomain.Name
            };
        }

        public AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherRequestDTO)
        {
            var publisherDomain = new Publishers
            {
                Name = addPublisherRequestDTO.Name
            };
            _dbContext.Publishers.Add(publisherDomain);
            _dbContext.SaveChanges();
            return addPublisherRequestDTO;
        }

        public PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO publisherNoIdDTO)
        {
            var publisherDomain = _dbContext.Publishers.FirstOrDefault(n => n.Id == id);
            if (publisherDomain != null)
            {
                publisherDomain.Name = publisherNoIdDTO.Name;
                _dbContext.SaveChanges();
            }
            return publisherNoIdDTO;
        }

        public Publishers? DeletePublisherById(int id)
        {
            var publisherDomain = _dbContext.Publishers.FirstOrDefault(n => n.Id == id);
            if (publisherDomain != null)
            {
                _dbContext.Publishers.Remove(publisherDomain);
                _dbContext.SaveChanges();
            }
            return publisherDomain;
        }
    }
}