using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuadrifoglioAPI.Migrations
{
    /// <inheritdoc />
    public partial class apitabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "121f66c1-4b2c-40dc-a453-a610b5c49ec9");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "121f66c1-4b2c-40dc-a453-a610b5c49ec9", 0, "0d6eceed-d156-4d6c-8872-37208421de75", "fredrik@user.com", true, "Fredrik", "User", false, null, "FREDRIK@USER.COM", "FREDRIK@USER.COM", "AQAAAAIAAYagAAAAEGFyHnikd5Yn9Z2Rl7FXHtHthObv4OzC41moAkc4tlKu4xeosBRMJUjx9dcHxJICrQ==", null, false, "59aa725d-305f-4303-adca-1e599a253103", false, "fredrik@user.com" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "City", "Country", "PostalCode", "State", "Street", "UserId" },
                values: new object[] { 2, "Hudiksvall", "Sweden", "824 43", "Gävleborg", "Hövdingegatan 13A", "121f66c1-4b2c-40dc-a453-a610b5c49ec9" });
        }
    }
}
