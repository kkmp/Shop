using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Shop.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext context;

        public RoleRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<IdentityRole> GetRoleByName(string roleName) => await context.Roles.FirstOrDefaultAsync(x => x.Name == roleName);

        public async Task<List<string>> GetUserIdsByRoleId(string roleId) => await context.UserRoles.Where(x => x.RoleId == roleId).Select(x => x.UserId).ToListAsync();
    }
}
