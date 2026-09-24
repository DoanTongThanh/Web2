using System.ComponentModel.DataAnnotations.Schema;

namespace Web2.Models.Domain
{
    public class Book_Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BookId { get; set; }
        public Books? Book { get; set; }
        public int AuthorId { get; set; }
        public Authors? Author { get; set; }
    }
}
