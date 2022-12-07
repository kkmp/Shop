using Microsoft.AspNetCore.Identity;

namespace Shop.Services
{
    public interface IGenerateJWT
    {
        string CreateToken(string username, string userId, string roleName);
    }
}
