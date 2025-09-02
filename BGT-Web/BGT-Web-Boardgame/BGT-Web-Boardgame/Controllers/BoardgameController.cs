using BGT_Web_Boardgame.Models.Dto;
using BGT_Web_Boardgame.Models.Request;
using BGT_Web_Boardgame.Models.Response;
using BGT_Web_Boardgame.Services;
using BGT_Web_Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BGT_Web_Boardgame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardgameController : ControllerBase
    {
        private readonly BoardgameService _boardgameService;
        private readonly UserExistProducer _userExistsConsumer;
        public BoardgameController(BoardgameService boardgameService, UserExistProducer userExistsConsumer)
        {
            _boardgameService = boardgameService;
            _userExistsConsumer = userExistsConsumer;
        }

        [HttpGet("hello")]
        public Task<string> Hello(string name)
        {
            return Task.FromResult("Hello" + name);
        }


        [HttpPost("Add-Boardgame")]
        public async Task<IActionResult> AddBoardgameAsync([FromBody] BoardgameRequest request)
        {
            var result =  await _boardgameService.AddAsync(request);

            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = 500,
                    error = result.Message,
                });
            }

            return Ok(new
            {
                status = 201,
                message = result.Message,
                data = result.Result
            });
        }

        [HttpPost("Add-Boardgame-Owner")]
        public async Task<IActionResult> AddBoardgameOwnerAsync(BoardgameOwnerRequest request)
        {
            throw new  NotImplementedException();
        }

        [HttpGet]
        public async Task<bool> CheckUser(int id = 1)
        {
            return await _userExistsConsumer.userExists(id);
        }

        [HttpGet("Get-Boardgames")]
        public async Task<ActionResult<PageWrapper<BoardgameDto>>> GetBoardgames([FromQuery] GetBoardgamesRequest request)
        {
            var result = await _boardgameService.getAllBoardgamesAsync(request);

            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = 500,
                    error = result.Message,
                });
            }

            return Ok(result);

        }
    }
}
