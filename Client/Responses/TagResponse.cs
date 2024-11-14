using System.ComponentModel.DataAnnotations;

namespace Api.Responses
{
    public class TagResponse
    {
        [Key]
        public int? TagId { get; set; }
        public string? TagName { get; set; }
        public string? Note { get; set; }
    }
}
