
using MassTransit;
using BGT_Web_Contracts;
using System.Threading.Tasks;

namespace BGT_Web_Boardgame.Services
{
    public class UserExistProducer
    {
        private readonly IRequestClient<UserExistsRequest> _client;
        public UserExistProducer(IRequestClient<UserExistsRequest> client)
        {
            _client = client;
        }

        public async Task<bool> userExists(int userId)
        {
            var response = await _client.GetResponse<UserExistsResponse>(new { UserId = userId });
            return response.Message.Exists;
        }


    }
}
