using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "70519e86-dc76-4851-acc1-7329dc8e45cd", null, "Admin", "ADMIN" },
                    { "a0075345-4251-4797-9d12-465d5391d554", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "ecee591a-54d7-4c6c-a4bb-8bae64f0db29", 0, "65ed4837-d31a-470a-8d92-11a701a32aa5", "user@bookstore.com", false, "API", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEA1YrfxszyJzntlynHyTMoVoNK2yMLKYtunZVaA51boKjihae1oNRyV9m4A7cbb7/w==", null, false, "4db047ad-4ccf-4d20-ab2c-24c24b70ff1c", false, "user@bookstore.com" },
                    { "f2b62630-46cd-44e6-938d-343704f5b4e1", 0, "743bdb52-c0bb-4bbe-ba65-b75bddbc997b", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEPJ7dsZJ8qOgSmkHIHSgyNQFRcikPYPRYUsx5GGL4NbcQ1yevc/icntZk7Ml68sl5w==", null, false, "2b7fc838-7052-40fe-ab70-7ac5b77522a8", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "a0075345-4251-4797-9d12-465d5391d554", "ecee591a-54d7-4c6c-a4bb-8bae64f0db29" },
                    { "70519e86-dc76-4851-acc1-7329dc8e45cd", "f2b62630-46cd-44e6-938d-343704f5b4e1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a0075345-4251-4797-9d12-465d5391d554", "ecee591a-54d7-4c6c-a4bb-8bae64f0db29" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "70519e86-dc76-4851-acc1-7329dc8e45cd", "f2b62630-46cd-44e6-938d-343704f5b4e1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70519e86-dc76-4851-acc1-7329dc8e45cd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0075345-4251-4797-9d12-465d5391d554");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ecee591a-54d7-4c6c-a4bb-8bae64f0db29");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f2b62630-46cd-44e6-938d-343704f5b4e1");
        }
    }
}
