using Web2.Models.Domain;
namespace Web2.Models.DTO
{
    public class BookDTO
    {
        public int Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public int? Rate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public string PublisherName { get; set; } = string.Empty;
        public List<string>? AuthorNames { get; set; }
    }
}
