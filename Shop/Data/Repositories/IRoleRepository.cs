using Microsoft.AspNetCore.Identity;

namespace Shop.Data.Repositories
{
    public interface IRoleRepository
    {
        Task<IdentityRole> GetRoleByName(string roleName);
        Task<List<string>> GetUserIdsByRoleId(string roleId);
    }
}
