using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop12.Data.Migrations
{
    /// <inheritdoc />
    public partial class mi21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Products_Id_Product",
                table: "Ratings");

            migrationBuilder.AlterColumn<int>(
                name: "Id_Product",
                table: "Ratings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Products_Id_Product",
                table: "Ratings",
                column: "Id_Product",
                principalTable: "Products",
                principalColumn: "Id_Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Products_Id_Product",
                table: "Ratings");

            migrationBuilder.AlterColumn<int>(
                name: "Id_Product",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Products_Id_Product",
                table: "Ratings",
                column: "Id_Product",
                principalTable: "Products",
                principalColumn: "Id_Product",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
