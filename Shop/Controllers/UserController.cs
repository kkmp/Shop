using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shop.Data.Repositories;
using Shop.Data.UnitOfWork;
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
        private readonly IGenerateJWT generateJWT;
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork unitOfWork, IGenerateJWT generateJWT)
        {
            this.unitOfWork = unitOfWork;
            this.generateJWT = generateJWT;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO login)
        {
            IActionResult response;
            var result = await unitOfWork.UserRepository.AuthenticateUser(login);

            if (result.Success)
            {
                var role = await unitOfWork.UserManager.GetRolesAsync(result.Data);
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
            return await ExecuteForResult(async () => await unitOfWork.UserRepository.CreateUser(user));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            return await ExecuteForResult(async () => await unitOfWork.UserRepository.DeleteUser(userId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("changeUserRole/{userId}")]
        public async Task<IActionResult> ChangeUserRole(Guid userId, IdDTO roleId)
        {
            return await ExecuteForResult(async () => await unitOfWork.UserRepository.ChangeUserRole(userId, roleId.Id.Value));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("showUsers")]
        public async Task<IActionResult> ShowUsers()
        {
            var result = await unitOfWork.UserRepository.GetNotAdminUsers();

            return Ok(new { users = unitOfWork.Mapper.Map<List<UserGetDTO>>(result.Data) });
        }
    }
}
