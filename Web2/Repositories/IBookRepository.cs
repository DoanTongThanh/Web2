using Web2.Models.Domain;
using Web2.Models.DTO;

namespace Web2.Repositories
{
    public interface IBookRepository
    {
        List<BookDTO> GetAllBooks(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000);

        BookDTO GetBookById(int id);
        addBookRequestDTO AddBook(addBookRequestDTO bookRequest);
        addBookRequestDTO UpdateBook(int id, addBookRequestDTO bookRequest);
        Books DeleteBook(int id);
    }
}