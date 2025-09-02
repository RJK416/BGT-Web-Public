using System.ComponentModel.DataAnnotations;

namespace BGT_Web_Account.Models.Request
{
    public class PasswordResetRequest
    {
        [Required, MinLength(3)]
        public string Username { get; set; }
    }
}
