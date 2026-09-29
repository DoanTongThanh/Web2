using Microsoft.EntityFrameworkCore;
using Web2.Data;
using Web2.Models.Domain;
using Web2.Models.DTO;

namespace Web2.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLBookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BookDTO> GetAllBooks(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var allBooks = _dbContext.Books
                .Select(book => new BookDTO()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.IsRead ? book.DateRead : null,
                    Rate = book.IsRead ? book.Rate : null,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    DateAdded = book.DateAdded,
                    PublisherName = book.Publisher != null ? book.Publisher.Name : "Unknown",
                    AuthorNames = book.Book_Authors != null
                        ? book.Book_Authors.Where(n => n.Author != null).Select(n => n.Author.FullName).ToList()
                        : new List<string>()
                }).AsQueryable();


            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = allBooks.Where(x => x.Title != null && x.Title.ToLower().Contains(filterQuery.ToLower()));
                }
                if (filterOn.Equals("description", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = allBooks.Where(x => x.Description != null && x.Description.ToLower().Contains(filterQuery.ToLower()));
                }

                if (filterOn.Equals("rate", StringComparison.OrdinalIgnoreCase) && int.TryParse(filterQuery, out var rateValue))
                {
                    allBooks = allBooks.Where(x => x.Rate == rateValue);
                }
            }


            var skipResults = (pageNumber - 1) * pageSize;

            return allBooks.Skip(skipResults).Take(pageSize).ToList();
        }

        public BookDTO GetBookById(int id)
        {
            var bookDTO = _dbContext.Books
                .Where(n => n.Id == id)
                .Select(book => new BookDTO()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.IsRead ? book.DateRead : null,
                    Rate = book.IsRead ? book.Rate : null,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    DateAdded = book.DateAdded,
                    PublisherName = book.Publisher != null ? book.Publisher.Name : "Unknown",
                    AuthorNames = book.Book_Authors != null
                        ? book.Book_Authors.Where(n => n.Author != null).Select(n => n.Author.FullName).ToList()
                        : new List<string>()
                }).FirstOrDefault();

            return bookDTO!;
        }

        public addBookRequestDTO AddBook(addBookRequestDTO addBookRequestDTO)
        {
            var bookDomainModel = new Books
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead = addBookRequestDTO.IsRead,
                DateRead = addBookRequestDTO.DateRead,
                Rate = addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverUrl = addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded,
                PublisherId = addBookRequestDTO.PublisherId
            };

            _dbContext.Books.Add(bookDomainModel);
            _dbContext.SaveChanges();

            
            if (addBookRequestDTO.AuthorIds != null)
            {
                foreach (var authorId in addBookRequestDTO.AuthorIds)
                {
                    var bookAuthorModel = new Book_Author()
                    {
                        BookId = bookDomainModel.Id,
                        AuthorId = authorId
                    };
                    _dbContext.Book_Authors.Add(bookAuthorModel);
                }
                _dbContext.SaveChanges();
            }

            return addBookRequestDTO;
        }

        public addBookRequestDTO UpdateBook(int id, addBookRequestDTO bookDTO)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null)
            {
                bookDomain.Title = bookDTO.Title;
                bookDomain.Description = bookDTO.Description;
                bookDomain.IsRead = bookDTO.IsRead;
                bookDomain.DateRead = bookDTO.DateRead;
                bookDomain.Rate = bookDTO.Rate;
                bookDomain.Genre = bookDTO.Genre;
                bookDomain.CoverUrl = bookDTO.CoverUrl;
                bookDomain.DateAdded = bookDTO.DateAdded;
                bookDomain.PublisherId = bookDTO.PublisherId;

                _dbContext.SaveChanges();
            }

            var existingBookAuthors = _dbContext.Book_Authors.Where(a => a.BookId == id).ToList();
            if (existingBookAuthors.Any())
            {
                _dbContext.Book_Authors.RemoveRange(existingBookAuthors);
                _dbContext.SaveChanges();
            }

            if (bookDTO.AuthorIds != null)
            {
                foreach (var authorId in bookDTO.AuthorIds)
                {
                    var bookAuthorModel = new Book_Author()
                    {
                        BookId = id,
                        AuthorId = authorId
                    };
                    _dbContext.Book_Authors.Add(bookAuthorModel);
                }

                _dbContext.SaveChanges();
            }
            return bookDTO;
        }

        public Books DeleteBook(int id)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null)
            {
                var existingBookAuthors = _dbContext.Book_Authors.Where(a => a.BookId == id).ToList();
                if (existingBookAuthors.Any())
                {
                    _dbContext.Book_Authors.RemoveRange(existingBookAuthors);
                }

                _dbContext.Books.Remove(bookDomain);
                _dbContext.SaveChanges();
            }
            return bookDomain!;
        }
    }
}