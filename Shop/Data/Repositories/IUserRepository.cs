using Microsoft.AspNetCore.Identity;
using Shop.DTO.User;

namespace Shop.Data.Repositories
{
    public interface IUserRepository
    {
        Task<DbResult<IdentityUser>> AuthenticateUser(UserLoginDTO login);
        Task<DbResult<IdentityUser>> CreateUser(UserCreateDTO user);
        Task<DbResult<IdentityUser>> DeleteUser(Guid userId);
        Task<DbResult<IdentityUser>> ChangeUserRole(Guid userId, Guid roleId);
        Task<DbResult<List<IdentityUser>>> GetNotAdminUsers();
        Task<IdentityUser> GetUserByName(string userName);
        Task<IdentityUser> GetUserById(Guid userId);
    };
}
