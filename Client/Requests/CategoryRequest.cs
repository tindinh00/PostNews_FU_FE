using System.ComponentModel.DataAnnotations;

namespace Client.Requests
{
    public class CategoryRequest
    {
        public short? CategoryId { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        [Required]
        public string? CategoryDesciption { get; set; }
    }
}
