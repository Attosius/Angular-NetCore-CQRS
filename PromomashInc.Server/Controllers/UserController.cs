
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PromomashInc.DataAccess.Context;
using PromomashInc.DataAccess.Models;
using PromomashInc.EntitiesDto;
using PromomashInc.Helpers.FunctionalResult;
using PromomashInc.Core;

namespace PromomashInc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
       
        private readonly ILogger<UserController> _logger;
        private readonly UserDataContext _userDataContext;
        private readonly IMapper _mapper;
        private readonly ICustomPasswordHasher _customPasswordHasher;

        public UserController(
            ILogger<UserController> logger,
            UserDataContext userDataContext,
            IMapper mapper,
            ICustomPasswordHasher customPasswordHasher
            )
        {
            _logger = logger;
            _userDataContext = userDataContext;
            _mapper = mapper;
            _customPasswordHasher = customPasswordHasher;
        }

        [HttpPost(nameof(Save))]
        public async Task<Result> Save([FromBody] UserDto userData)
        {
            try
            {
                if (userData == null)
                {
                    return Result.Fail("User data undefined");
                }

                var user = _mapper.Map<User>(userData);
                var isNew = user.Id == 0;
                
                var hashed = _customPasswordHasher.GetHash(userData.Password);
                user.PasswordHash = hashed;
                _userDataContext.Entry(user).State = isNew ?
                    EntityState.Added :
                    EntityState.Modified;
                await _userDataContext.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Results error");
                return e.ToErrorResult<Result>($"Error: {e.Message}");
            }
        }
    }
}
