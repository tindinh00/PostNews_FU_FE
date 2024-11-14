using System.ComponentModel.DataAnnotations;

namespace Api.Responses
{
    public class CategoryResponse
    {
        [Key]
        public short? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDesciption { get; set; }
        public List<NewsResponse>? News { get; set; }
    }
}
