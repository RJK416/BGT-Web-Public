namespace BGT_Web_Boardgame.Models.Models
{
    public class BoardgameOwnerModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int BoardgameId { get; set; }
        public BoardgameModel Boardgame { get; set; } = null!;

        public DateTime OwnerDate { get; set; } = DateTime.Now;

        public bool IsLoaned { get; set; }
    }
}
