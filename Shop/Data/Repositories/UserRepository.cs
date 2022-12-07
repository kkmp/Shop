using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.DTO.User;
using SSC.Data.Repositories;

namespace Shop.Data.Repositories
{
    public class UserRepository : BaseRepository<IdentityUser>, IUserRepository
    {
        private readonly DataContext context;
        private readonly IRoleRepository roleRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public UserRepository(DataContext context, IMapper mapper, IRoleRepository roleRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.roleRepository = roleRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<DbResult<IdentityUser>> AuthenticateUser(UserLoginDTO login)
        {
            var user = await GetUserByEmail(login.Email);

            var conditions = new Dictionary<Func<bool>, string>
            {
                { () => user == null, "User does not exist" }
            };

            var result = Validate(conditions);
            if (result != null)
            {
                return result;
            }

            var signInResult = await signInManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: false); //metody tego typu

            if(signInResult.Succeeded)
            {
                return DbResult<IdentityUser>.CreateSuccess("Authentication success", user);
            }
            else
            {
                return DbResult<IdentityUser>.CreateFail("Incorrect password");
            }    
        }

        public async Task<DbResult<IdentityUser>> CreateUser(UserCreateDTO user)
        {
            Dictionary<Func<bool>, string> conditions = new Dictionary<Func<bool>, string>
            {
                { () => GetUserByName(user.UserName).Result != null, "Username already exists" },
                { () => GetUserByEmail(user.Email).Result != null, "Email has already been used" },
                { () => user.Password1 != user.Password2, "Passwords do not match" }
            };

            var result = Validate(conditions);
            if (result != null)
            {
                return result;
            }

            var newUser = mapper.Map<IdentityUser>(user);

            newUser.EmailConfirmed = true;

            var createResult = await userManager.CreateAsync(newUser, user.Password1);

            if(createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, RoleOptions.User);
                return DbResult<IdentityUser>.CreateSuccess("User created", newUser);
            }
            else
            {
                return DbResult<IdentityUser>.CreateFail(string.Join(", ", createResult.Errors.Select(x => x.Description)));
            }
           
        }

        public async Task<DbResult<IdentityUser>> DeleteUser(Guid userId)
        {
            var user = await GetUserById(userId);

            var conditions = new Dictionary<Func<bool>, string>
            {
                { () => user == null, "User does not exist" }
            };

            var result = Validate(conditions);
            if (result != null)
            {
                return result;
            }

            context.Remove(user);
            await context.SaveChangesAsync();

            return DbResult<IdentityUser>.CreateSuccess("User has been deleted", user);
        }

        public async Task<DbResult<List<IdentityUser>>> GetNotAdminUsers()
        {
            var role = await roleRepository.GetRoleByName(RoleOptions.User);
            var usersId = await roleRepository.GetUserIdsByRoleId(role.Id);

            var userList = await context.Users.Where(x => usersId.Contains(x.Id)).ToListAsync();

            return DbResult<List<IdentityUser>>.CreateSuccess("Success", userList);
        }

        public async Task<DbResult<IdentityUser>> ChangeUserRole(Guid userId, Guid roleId)
        {
            var user = await GetUserById(userId);
            var oldRoles = await userManager.GetRolesAsync(user);
            var roleToCheck = await roleManager.FindByIdAsync(roleId.ToString());

            var conditions = new Dictionary<Func<bool>, string>
            {
                { () => user == null, "User does not exist" },
                { () => roleToCheck == null, "Role does not exist" }
            };

            var result = Validate(conditions);
            if (result != null)
            {
                return result;
            }

            await userManager.RemoveFromRolesAsync(user, oldRoles);
            var call = await userManager.AddToRoleAsync(user, roleToCheck.Name);

            if (call.Succeeded) {
                return DbResult<IdentityUser>.CreateSuccess("Role has been changed", user);
            } 
            else
            {
                return DbResult<IdentityUser>.CreateFail("Cannot change user role");
            }
        }

        public async Task<IdentityUser> GetUserById(Guid userId) => await userManager.FindByIdAsync(userId.ToString());

        public async Task<IdentityUser> GetUserByName(string userName) => await userManager.FindByNameAsync(userName);

        private async Task<IdentityUser> GetUserByEmail(string userEmail) => await userManager.FindByEmailAsync(userEmail);
    }
}
