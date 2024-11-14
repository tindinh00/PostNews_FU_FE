using System.ComponentModel.DataAnnotations;

namespace Api.Responses
{
    public class NewsResponse
    {
        [Key]
        public string? NewsArticleId { get; set; }
        public string? NewsTitle { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? NewsContent { get; set; }
        public short? CategoryId { get; set; }
        public bool? NewsStatus { get; set; }
        public short? CreatedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Category { get; set; }
        public string? CreatedBy { get; set; }
        public string? Tags { get; set; }
    }
}
