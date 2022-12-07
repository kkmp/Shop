using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shop.DTO.User;

namespace Shop.Services
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<UserCreateDTO, IdentityUser>();

            CreateMap<IdentityUser, UserGetDTO>();
        }
    }
}
