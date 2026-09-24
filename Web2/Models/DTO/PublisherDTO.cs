namespace Web2.Models.DTO
{
    public class PublisherDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class PublisherNoIdDTO
    {
        public string Name { get; set; } = string.Empty;
    }
    
    public class PublisherWithBooksAndAuthorsDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<BookAuthorDTO>? BookAuthors { set; get; }
    }
    public class BookAuthorDTO
    {
        public string? BookName { get; set; }
        public List<string>? BookAuthors { get; set; }
    }
}
