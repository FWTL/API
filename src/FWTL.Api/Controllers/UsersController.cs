using FWTL.Core.Commands;
using FWTL.Core.Queries;
using FWTL.Core.Services;
using FWTL.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FWTL.Auth.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICurrentUserService _currentUserService;

        public UsersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICurrentUserService currentUserService)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task Register(RegisterUser.Request request)
        {
            await _commandDispatcher.DispatchAsync<RegisterUser.Request, RegisterUser.Command>(request);
        }

        //[HttpGet("Me")]
        //[Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
        //public async Task<GetMe.Result> Me()
        //{
        //    return await _queryDispatcher.DispatchAsync<GetMe.Query, GetMe.Result>(new GetMe.Query(_currentUserService));
        //}

        //[HttpPost("Me/Link/Telegram")]
        //[Authorize]
        //public async Task PhoneLogin(string phoneNumber)
        //{
        //    await _commandDispatcher.DispatchAsync<LinkTelegramAccount.Request, LinkTelegramAccount.Command>(new LinkTelegramAccount.Request() { PhoneNumber = phoneNumber }, command => command.NormalizePhoneNumber());
        //}

        //[Authorize]
        //[HttpPatch("Me/Timezone")]
        //public async Task AssignTimeZoneToUser(string zoneId)
        //{
        //    await _commandDispatcher.DispatchAsync<AssignTimeZoneToUser.Request, AssignTimeZoneToUser.Command>(new AssignTimeZoneToUser.Request() { ZoneId = zoneId });
        //}
    }
}