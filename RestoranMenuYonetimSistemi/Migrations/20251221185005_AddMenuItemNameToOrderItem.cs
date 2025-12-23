using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestoranMenuYonetimSistemi.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuItemNameToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MenuItemName",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuItemName",
                table: "OrderItems");
        }
    }
}
