using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shop.Data.Repositories;
using Shop.DTO;
using Shop.DTO.User;
using Shop.Services;
using SSC.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : CommonController
    {
        private IConfiguration _config;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IGenerateJWT generateJWT;

        public UserController(IConfiguration config,
            IUserRepository userRepository,
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            IGenerateJWT generateJWT)
        {
            _config = config;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.generateJWT = generateJWT;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO login)
        {
            IActionResult response;
            var result = await userRepository.AuthenticateUser(login);

            if (result.Success)
            {
                var role = await userManager.GetRolesAsync(result.Data);
                if (role.Count == 0)
                {
                    return BadRequestErrorMessage(result.Message);
                }

                var tokenString = generateJWT.CreateToken(result.Data.UserName, result.Data.Id, role[0]);
                response = Ok(new { token = tokenString });
            }
            else
            {
                response = BadRequestErrorMessage(result.Message);
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDTO user)
        {
            return await ExecuteForResult(async () => await userRepository.CreateUser(user));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            return await ExecuteForResult(async () => await userRepository.DeleteUser(userId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("changeUserRole/{userId}")]
        public async Task<IActionResult> ChangeUserRole(Guid userId, IdDTO roleId)
        {
            return await ExecuteForResult(async () => await userRepository.ChangeUserRole(userId, roleId.Id.Value));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("showUsers")]
        public async Task<IActionResult> ShowUsers()
        {
            var result = await userRepository.GetNotAdminUsers();

            return Ok(new { users = mapper.Map<List<UserGetDTO>>(result.Data) });
        }
    }
}
