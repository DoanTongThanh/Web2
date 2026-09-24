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

        public List<BookDTO> GetAllBooks()
        {
            return _dbContext.Books
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
                }).ToList();
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