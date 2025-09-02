using System.ComponentModel.DataAnnotations;

namespace BGT_Web_Account.Models.Request
{
    public class AccountCreateRequest
    {
        [Required, EmailAddress]
        public string? Mail { get; set; }

        [Required, MinLength(3)]
        public string? Username { get; set; }

        [Required, MinLength(6)]
        public string? Password { get; set; }
    }
}
