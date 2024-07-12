namespace PromomashInc.Core.Models;

public interface ICustomPasswordHasher
{
    string GetHash(string password);
}