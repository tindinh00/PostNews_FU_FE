using System.ComponentModel.DataAnnotations;

namespace Api.Responses
{
    public class AccountResponse
    {
        [Key]
        public short? AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public int? AccountRole { get; set; }
        public string? AccountPassword { get; set; }
    }
}
