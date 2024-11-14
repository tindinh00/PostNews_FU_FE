using System.ComponentModel.DataAnnotations;

namespace Client.Requests
{
    public class NewsRequest
    {
        public string? NewsArticleId { get; set; }

        [Required]
        public string? NewsTitle { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public string? NewsContent { get; set; }

        [Required]
        public short? CategoryId { get; set; }

        [Required]
        public bool? NewsStatus { get; set; } = false;

        public short? CreatedById { get; set; }

        public DateTime? ModifiedDate { get; set; } = DateTime.Now;

        public string[]? ListTag { get; set; }
    }
}
