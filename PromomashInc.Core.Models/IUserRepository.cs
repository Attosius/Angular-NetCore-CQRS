using PromomashInc.EntitiesDto;
using PromomashInc.Helpers.FunctionalResult;

namespace PromomashInc.Core.Models;

public interface IUserRepository
{
    public Task<Result> Save(UserDto userData);
}