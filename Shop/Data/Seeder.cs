using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Services;
using System.Security.Cryptography;

namespace Shop.Data
{
    public static class Seeder
    {
        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(new[]
            {
                new IdentityRole {Id = "64afcb40-4e45-4b94-9f31-19b57c20027f", Name = RoleOptions.User, NormalizedName = RoleOptions.User.ToUpper()},
                new IdentityRole {Id = "0c051c70-61a2-44bb-bf9c-691bf30a9c11", Name = RoleOptions.Admin, NormalizedName = RoleOptions.Admin.ToUpper()}
            });
        }

        public static void SeedUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUser>().HasData(MigrationUsers.Select(x => new IdentityUser
            {
                Email = x.Email,
                UserName = x.UserName,
                NormalizedEmail = x.Email.ToUpper(),
                NormalizedUserName = x.UserName.ToUpper(),
                Id = x.Id,
                ConcurrencyStamp = x.ConcurrencyStamp,
                EmailConfirmed = true,
                SecurityStamp = x.SecurityStamp,
                PasswordHash = x.Password
            }).ToArray());
        }

        public static void SeedUserRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(MigrationUsers.Select(x => new IdentityUserRole<string> { UserId = x.Id, RoleId = x.RoleId }));
        }

        public record MigrationUser(string ConcurrencyStamp, string Email, string Id, string UserName, string Password, string SecurityStamp, string RoleId);

        public static List<MigrationUser> MigrationUsers { get; } = new List<MigrationUser> {
            new MigrationUser
            (
                ConcurrencyStamp: "890598d3-2668-4110-9dd0-cf09985bc5ff",
                Email: "admin@gmail.com",
                Id: "3e7e4cc3-d3e1-4917-9647-42422b4c429a",
                UserName: "Admin",
                Password: PasswordHasher.HashPassword("password123"),
                SecurityStamp: "2d27171c-2299-4514-832f-99059b2b7b6d",
                RoleId: "0c051c70-61a2-44bb-bf9c-691bf30a9c11"
            ),
            new MigrationUser
            (
                ConcurrencyStamp: "f1fec69f-b6e5-4a89-8201-43fe16ad4cbf",
                Email: "user@gmail.com",
                Id: "a74929ae-95de-44ee-bb50-d3437de842dd",
                UserName: "User",
                Password: PasswordHasher.HashPassword("password123"),
                SecurityStamp: "d77fce18-5b5e-4d78-9a86-2b9c6f495013",
                RoleId: "64afcb40-4e45-4b94-9f31-19b57c20027f"
            )
        };
    }
}
