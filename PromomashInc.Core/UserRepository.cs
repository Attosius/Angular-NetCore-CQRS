using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PromomashInc.DataAccess.Context;
using PromomashInc.EntitiesDto;
using Microsoft.Extensions.Logging;
using PromomashInc.Core.Models;
using PromomashInc.DataAccess.Models;
using PromomashInc.Helpers.FunctionalResult;

namespace PromomashInc.Core
{
#nullable disable
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly UserDataContext _userDataContext;
        private readonly IMapper _mapper;
        private readonly ICustomPasswordHasher _customPasswordHasher;

        public UserRepository(ILogger<UserRepository> logger,
            UserDataContext userDataContext,
            IMapper mapper,
            ICustomPasswordHasher customPasswordHasher)
        {
            _logger = logger;
            _userDataContext = userDataContext;
            _mapper = mapper;
            _customPasswordHasher = customPasswordHasher;
        }

        public async Task<Result> Save(UserDto userData)
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
