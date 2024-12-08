using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop12.Data.Migrations
{
    /// <inheritdoc />
    public partial class mi99 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_Id_Product",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "Id_Product",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_Id_Product",
                table: "Reviews",
                column: "Id_Product",
                principalTable: "Products",
                principalColumn: "Id_Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_Id_Product",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "Id_Product",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_Id_Product",
                table: "Reviews",
                column: "Id_Product",
                principalTable: "Products",
                principalColumn: "Id_Product",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
