namespace BGT_Web_Account.Models.Entity
{
    public class PendingAccountModel
    {
        public required string userName { get; set; }
        public required string userEmail { get; set; }
        public required string passwordHash { get; set; }

        public string? otp {  get; set; }
        public DateTime? expiresAt { get; set; }
    }
}
