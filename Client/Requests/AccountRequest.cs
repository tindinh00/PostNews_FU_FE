using Client.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Requests
{
    public class AccountRequest
    {
        public short? AccountId { get; set; }
        [Required]
        public string? AccountName { get; set; }
        [Required]
        public string? AccountEmail { get; set; }
        public int? AccountRole { get; set; } = (int) RoleEnum.Staff;
    }
}
