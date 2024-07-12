
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PromomashInc.DataAccess.Context;
using PromomashInc.DataAccess.Models;
using PromomashInc.EntitiesDto;
using PromomashInc.Helpers.FunctionalResult;
using PromomashInc.Core;
using PromomashInc.Core.Models;

namespace PromomashInc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly UserDataContext _userDataContext;
        private readonly IMapper _mapper;
        private readonly ICustomPasswordHasher _customPasswordHasher;

        public UserController(
            ILogger<UserController> logger,
            IUserRepository userRepository
            )
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost(nameof(Save))]
        public async Task<Result> Save([FromBody] UserDto userData)
        {
            return await _userRepository.Save(userData);
        }
    }
}
