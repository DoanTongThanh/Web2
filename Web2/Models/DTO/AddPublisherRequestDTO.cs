using System.ComponentModel.DataAnnotations;

namespace Web2.Models.DTO
{
    public class AddPublisherRequestDTO
    {
        [Required(ErrorMessage = "Tên nhà xuất bản là bắt buộc")]
        [MinLength(3, ErrorMessage = "Tên nhà xuất bản phải có ít nhất 3 ký tự")]
        public string Name { get; set; } = string.Empty;
    }
}