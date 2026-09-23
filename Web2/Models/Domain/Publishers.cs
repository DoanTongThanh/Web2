using System.ComponentModel.DataAnnotations;

namespace Web2.Models.Domain
{
    public class Publishers
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Books>? Books { get; set; }
    }
}
