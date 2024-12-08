using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop12.Data.Migrations
{
    /// <inheritdoc />
    public partial class mi98 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_Id_Product",
                table: "Reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_Id_Product",
                table: "Reviews",
                column: "Id_Product",
                principalTable: "Products",
                principalColumn: "Id_Product",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_Id_Product",
                table: "Reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_Id_Product",
                table: "Reviews",
                column: "Id_Product",
                principalTable: "Products",
                principalColumn: "Id_Product");
        }
    }
}
