using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromomashInc.Server.Context;
using PromomashInc.Server.Dto;
using System.Security.Cryptography;
using AutoMapper;
using Helpers.FunctionalResult;

namespace PromomashInc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
       
        private readonly ILogger<UserController> _logger;
        private readonly BloggingContext _bloggingContext;
        private readonly IMapper _mapper;

        public UserController(
            ILogger<UserController> logger,
            BloggingContext bloggingContext,
            IMapper mapper
            )
        {
            _logger = logger;
            _bloggingContext = bloggingContext;
            _mapper = mapper;
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

                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
                Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: userData.Password!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
                user.PasswordHash = hashed;
                Console.WriteLine($"Hashed: {hashed}");

                var notes = _bloggingContext.Entry(user).State = isNew ?
                    EntityState.Added :
                    EntityState.Modified;
                await _bloggingContext.SaveChangesAsync();

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