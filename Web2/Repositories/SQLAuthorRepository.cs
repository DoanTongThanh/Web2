using Web2.Data;
using Web2.Models.Domain;
using Web2.Models.DTO;

namespace Web2.Repositories
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLAuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<AuthorDTO> GetAllAuthors(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var allAuthors = _dbContext.Authors
                .Select(author => new AuthorDTO()
                {
                    Id = author.Id,
                    FullName = author.FullName
                }).AsQueryable();

            
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("fullName", StringComparison.OrdinalIgnoreCase))
                {
                    allAuthors = allAuthors.Where(x => x.FullName != null && x.FullName.ToLower().Contains(filterQuery.ToLower()));
                }
            }

            
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("fullName", StringComparison.OrdinalIgnoreCase))
                {
                    allAuthors = isAscending ? allAuthors.OrderBy(x => x.FullName) : allAuthors.OrderByDescending(x => x.FullName);
                }
            }

            
            var skipResults = (pageNumber - 1) * pageSize;

            return allAuthors.Skip(skipResults).Take(pageSize).ToList();
        }

        public AuthorNoIdDTO GetAuthorById(int id)
        {
            
            var authorWithIdDomain = _dbContext.Authors.FirstOrDefault(x => x.Id == id);
            if (authorWithIdDomain == null)
            {
                return null!;
            }
            
            var authorNoIdDTO = new AuthorNoIdDTO
            {
                FullName = authorWithIdDomain.FullName,
            };
            return authorNoIdDTO;
        }

        public AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorDomainModel = new Authors
            {
                FullName = addAuthorRequestDTO.FullName,
            };
            
            _dbContext.Authors.Add(authorDomainModel);
            _dbContext.SaveChanges();
            return addAuthorRequestDTO;
        }

        public AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (authorDomain != null)
            {
                authorDomain.FullName = authorNoIdDTO.FullName;
                _dbContext.SaveChanges();
            }
            return authorNoIdDTO;
        }

        public Authors? DeleteAuthorById(int id)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (authorDomain != null)
            {
                _dbContext.Authors.Remove(authorDomain);
                _dbContext.SaveChanges();
            }
            return null;
        }
    }
}