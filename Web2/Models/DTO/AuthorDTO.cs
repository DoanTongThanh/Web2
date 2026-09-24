using Web2.Models.Domain;
namespace Web2.Models.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
    public class AuthorNoIdDTO
    {
        public string FullName { get; set; }= string.Empty;
    }
}
