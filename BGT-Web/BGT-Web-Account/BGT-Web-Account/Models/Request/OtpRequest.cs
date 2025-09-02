using System.ComponentModel.DataAnnotations;

namespace BGT_Web_Account.Models.Request
{
    public class OtpRequest
    {
        [Required, MinLength(6)]
        public string Otp {  get; set; }

        [Required, MinLength(3)]
        public string UserName { get; set; }
    }
}
