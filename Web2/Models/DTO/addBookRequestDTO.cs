using Web2.Models.Domain;
namespace Web2.Models.DTO
{
    public class addBookRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public int? Rate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public int PublisherId { get; set; }
        public List<int> AuthorIds { get; set; } = new List<int>();
    }
}
