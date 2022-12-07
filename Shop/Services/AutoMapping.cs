using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shop.Data.Models;
using Shop.DTO.Order;
using Shop.DTO.Product;
using Shop.DTO.User;

namespace Shop.Services
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<UserCreateDTO, IdentityUser>();
            CreateMap<ProductCreateDTO, Product>();
            CreateMap<OrderCreateDTO, Order>();

            CreateMap<IdentityUser, UserGetDTO>();
            CreateMap<Product, ProductGetDTO>();
            CreateMap<Order, OrderGetDTO>();

            CreateMap<ProductUpdateDTO, Product>();
        }
    }
}
