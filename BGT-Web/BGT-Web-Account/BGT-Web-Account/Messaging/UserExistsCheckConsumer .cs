using BGT_Web_Account.Interfaces;
using BGT_Web_Account.Repository;
using BGT_Web_Contracts;
using MassTransit;


namespace BGT_Web_Account.Messaging
{
    public class UserExistsCheckConsumer : IConsumer<UserExistsRequest>
    {
        private readonly IAccountRepository _userRepository;

        public UserExistsCheckConsumer(IAccountRepository  userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<UserExistsRequest> context)
        {
            var userId = context.Message.UserId;
            var exists = await _userRepository.userExistsAsync(userId); // bool

            await context.RespondAsync<UserExistsResponse>(new { Exists = exists });

        }
    }
}
