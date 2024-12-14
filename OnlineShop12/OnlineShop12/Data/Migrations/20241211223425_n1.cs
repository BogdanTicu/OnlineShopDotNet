using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop12.Data.Migrations
{
    /// <inheritdoc />
    public partial class n1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Products_Id_Product",
                table: "Ratings");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Products_Id_Product",
                table: "Ratings",
                column: "Id_Product",
                principalTable: "Products",
                principalColumn: "Id_Product",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Products_Id_Product",
                table: "Ratings");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Products_Id_Product",
                table: "Ratings",
                column: "Id_Product",
                principalTable: "Products",
                principalColumn: "Id_Product");
        }
    }
}
