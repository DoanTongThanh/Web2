using System.ComponentModel.DataAnnotations;

namespace Web2.Models.Domain
{
    public class Authors
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public List<Book_Author>? Book_Authors { get; set; }
    }
}
