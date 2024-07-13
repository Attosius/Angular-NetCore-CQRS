
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using PromomashInc.DataAccess.Context;
using PromomashInc.EntitiesDto;
using PromomashInc.Helpers.FunctionalResult;
using PromomashInc.Core.Models;
using MediatR;
using PromomashInc.EntitiesDto.Command;

namespace PromomashInc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly UserDataContext _userDataContext;
        private readonly IMapper _mapper;
        private readonly ICustomPasswordHasher _customPasswordHasher;

        public UserController(
            ILogger<UserController> logger,
            IMapper mapper,
            IUserRepository userRepository,
            IMediator mediator
            )
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
            _mediator = mediator;
        }

        [HttpPost(nameof(Save))]
        public async Task<Result<int>> Save([FromBody] UserDto userData)
        {
            try
            {
                var createUserCommand = _mapper.Map<CreateUserCommand>(userData);
                var id = await _mediator.Send(createUserCommand);
                return id.ToSuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Results error");
                return e.ToErrorResult<int>(e.Message);
            }
        }
    }
}
