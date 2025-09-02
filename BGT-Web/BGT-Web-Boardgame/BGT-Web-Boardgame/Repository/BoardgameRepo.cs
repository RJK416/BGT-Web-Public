using BGT_Web_Boardgame.Data;
using BGT_Web_Boardgame.Models.Dto;
using BGT_Web_Boardgame.Models.Models;
using BGT_Web_Boardgame.Models.Request;
using BGT_Web_Boardgame.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace BGT_Web_Boardgame.Repository
{
    public class BoardgameRepo
    {
        private readonly BoardgameDbContext _dbContext;

        public BoardgameRepo(BoardgameDbContext dbContext)
        {
             _dbContext = dbContext;
        }

        public async Task<bool> AddAsync(BoardgameModel boardgame)
        {    
            _dbContext.Boardgames.Add(boardgame);
            await _dbContext.SaveChangesAsync();


            return true;
        }

        public async Task<bool> BoardGameExistsByNameAsync(string name)
        {
            return await _dbContext.Boardgames.AnyAsync(b => b.Name == name);
        }

        public async Task<PageWrapper<BoardgameDto>> getAllBoardgamesAsync(GetBoardgamesRequest getBoardgamesRequest)
        {

            var results = await _dbContext.Boardgames
                                .AsNoTracking()
                                .OrderBy(b => b.Id)
                                .Skip((getBoardgamesRequest.Page - 1) * getBoardgamesRequest.PageSize)
                                .Take(getBoardgamesRequest.PageSize)
                                .Select(b => new BoardgameDto
                                {
                                    Name = b.Name,
                                    Description = b.Description,
                                    MaxPlayers = b.MaxPlayers,
                                    MinPlayers = b.MinPlayers,
                                    Diff = b.Diff,
                                    SoloWinner = b.SoloWinner,
                                    Teamplay  = b.Teamplay,
                                })
                                .ToListAsync();

            return new PageWrapper<BoardgameDto>()
            {
                IsSuccess = true,
                Message = "Successfuly retrieved boardgames.",
                Items = results
            };

        }
    }
}
