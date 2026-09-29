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

        public List<PublisherDTO> GetAllPublishers(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var allPublishers = _dbContext.Publishers
                .Select(publisher => new PublisherDTO()
                {
                    Id = publisher.Id,
                    Name = publisher.Name
                }).AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    allPublishers = allPublishers.Where(x => x.Name != null && x.Name.ToLower().Contains(filterQuery.ToLower()));
                }
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    allPublishers = isAscending ? allPublishers.OrderBy(x => x.Name) : allPublishers.OrderByDescending(x => x.Name);
                }
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return allPublishers.Skip(skipResults).Take(pageSize).ToList();
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