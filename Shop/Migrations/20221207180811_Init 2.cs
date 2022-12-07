using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0c051c70-61a2-44bb-bf9c-691bf30a9c11", "9e3852a6-a1de-4a76-86a6-6b3f76494589", "Admin", "ADMIN" },
                    { "64afcb40-4e45-4b94-9f31-19b57c20027f", "bdc05980-b9d9-4cae-bd27-667418cca126", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "3e7e4cc3-d3e1-4917-9647-42422b4c429a", 0, "890598d3-2668-4110-9dd0-cf09985bc5ff", "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", "ADMIN", "AOjdNDHuVckmr91aOVcEDYNAx2dK8eblmZ8Ez79KLQ9WnQhK+ah9tZCzx0dQQTqIYw==", null, false, "2d27171c-2299-4514-832f-99059b2b7b6d", false, "Admin" },
                    { "a74929ae-95de-44ee-bb50-d3437de842dd", 0, "f1fec69f-b6e5-4a89-8201-43fe16ad4cbf", "user@gmail.com", true, false, null, "USER@GMAIL.COM", "USER", "APukIh2yY+RScGcV1dHMl+yunUcuM0O6Vx+ot+r/PkEDYBOa+9G0JnlupusTW/EXSQ==", null, false, "d77fce18-5b5e-4d78-9a86-2b9c6f495013", false, "User" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0c051c70-61a2-44bb-bf9c-691bf30a9c11", "3e7e4cc3-d3e1-4917-9647-42422b4c429a" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "64afcb40-4e45-4b94-9f31-19b57c20027f", "a74929ae-95de-44ee-bb50-d3437de842dd" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0c051c70-61a2-44bb-bf9c-691bf30a9c11", "3e7e4cc3-d3e1-4917-9647-42422b4c429a" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "64afcb40-4e45-4b94-9f31-19b57c20027f", "a74929ae-95de-44ee-bb50-d3437de842dd" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c051c70-61a2-44bb-bf9c-691bf30a9c11");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64afcb40-4e45-4b94-9f31-19b57c20027f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3e7e4cc3-d3e1-4917-9647-42422b4c429a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a74929ae-95de-44ee-bb50-d3437de842dd");
        }
    }
}
