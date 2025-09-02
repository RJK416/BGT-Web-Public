using System.ComponentModel.DataAnnotations;

namespace BGT_Web_Account.Models.Request
{
    public class LoginRequest
    {
        [Required, MinLength(3)]
        public string? Username { get; set; }

        [Required, MinLength(6)]
        public string? Password { get; set; }

        public int ExpiriationTime { get; set; } 

    }
}
