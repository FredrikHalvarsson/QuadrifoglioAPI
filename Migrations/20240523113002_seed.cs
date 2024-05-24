using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuadrifoglioAPI.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RestaurantAddress",
                columns: new[] { "Id", "City", "PostalCode", "Street" },
                values: new object[,]
                {
                    { 2, "Sundsvall", "852 30", "Storgatan 6" },
                    { 3, "Örnsköldsvik", "891 63", "Köpmangatan 3A" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "AddressId", "Name" },
                values: new object[,]
                {
                    { 2, 2, "IlQuadrifoglio - Sundsvall" },
                    { 3, 3, "IlQuadrifoglio - Övik" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RestaurantAddress",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RestaurantAddress",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
