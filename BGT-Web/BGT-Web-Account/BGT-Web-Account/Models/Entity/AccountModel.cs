namespace BGT_Web_Account.Models.Entity
{
    public class AccountModel
    {
        public int Id { get; set; }
        public required string userName { get; set; } = "";

        public required string userEmail { get; set; } = "";

        public required string passwordHash { get; set; } = "";

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public bool isActive { get; set; }

        public string userType { get; set; } = "user";
    }
}
