using BGT_Web_Boardgame.Enum;

namespace BGT_Web_Boardgame.Models.Request
{
    public class BoardgameRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MaxPlayers { get; set; }

        public int MinPlayers { get; set; }

        public diff Diff { get; set; }

        public bool SoloWinner { get; set; }

        public bool Teamplay { get; set; }
    }
}
