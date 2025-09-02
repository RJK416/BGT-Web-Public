using BGT_Web_Boardgame.Models.Request;
using BGT_Web_Boardgame.Repository;
using BGT_Web_Boardgame.Models.Models;
using BGT_Web_Boardgame.Models.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using BGT_Web_Boardgame.Models.Dto;

namespace BGT_Web_Boardgame.Services
{
    public class BoardgameService
    {
        private readonly BoardgameRepo _repo;

        public BoardgameService(BoardgameRepo repo)
        {
             _repo = repo;
        }

        public async Task<ResultWrapper<bool>> AddAsync(BoardgameRequest request)
        {
            var boardgameExists = await _repo.BoardGameExistsByNameAsync(request.Name);

            if (boardgameExists)
            {
                return ResultWrapper<bool>.Fail(false, "Boardgame already added");
            }

            var boardgame = new BoardgameModel()
            {
                Name = request.Name,
                Description = request.Description,
                MaxPlayers = request.MaxPlayers,
                MinPlayers = request.MinPlayers,
                Diff = request.Diff,
                SoloWinner = request.SoloWinner,
                Teamplay = request.Teamplay,
            };

            bool result = await _repo.AddAsync(boardgame);

            if (!result)
            {
                return ResultWrapper<bool>.Fail(false, "Intenal Error");
            }

            return ResultWrapper<bool>.Ok(result,"Boardgame successfuly added.");

        }

        public async Task<PageWrapper<BoardgameDto>> getAllBoardgamesAsync(GetBoardgamesRequest getBoardgamesRequest)
        {
            if (getBoardgamesRequest.PageSize < 0 || getBoardgamesRequest.Page < 0 )
            {
                return PageWrapper<BoardgameDto>.Fail(null, "Requested PageSize and Page cannot be less than 0 ");
            }

            var results = await _repo.getAllBoardgamesAsync(getBoardgamesRequest);

            if ( !results.IsSuccess) 
            {
                return PageWrapper<BoardgameDto>.Fail(null, "Failed to retrieve Boardgames");

            }

            return results;
        }

    }
}
