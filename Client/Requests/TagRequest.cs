using System.ComponentModel.DataAnnotations;

namespace Client.Requests
{
    public class TagRequest
    {
        public int? TagId { get; set; }

        [Required]
        public string? TagName { get; set; }
        [Required]
        public string? Note { get; set; }
    }
}
