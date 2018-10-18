using Surgicalogic.Data.Entities;

namespace Surgicalogic.Contracts.Services
{
    public interface ITokenService
    {
        object GenerateToken(string email, User user);
        string GetRefreshToken(string email, string token);
        string GetEmailFromExpiredToken(string token);
    }
}