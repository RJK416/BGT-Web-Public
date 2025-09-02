namespace BGT_Web_Boardgame.Models.Request
{
    public class GetBoardgamesRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
