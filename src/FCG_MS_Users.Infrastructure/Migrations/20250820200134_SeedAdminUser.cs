using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG_MS_Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "fcg_user",
                table: "userAuthorizations",
                columns: new[] { "id", "permission", "UserId" },
                values: new object[] { new Guid("95cbf698-2295-4ee5-a2fe-1d4bde8a6479"), "Admin", new Guid("bacbbe47-017e-49a0-bd1a-5bbc2a2ffaca") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "fcg_user",
                table: "userAuthorizations",
                keyColumn: "id",
                keyValue: new Guid("95cbf698-2295-4ee5-a2fe-1d4bde8a6479"));
        }
    }
}
