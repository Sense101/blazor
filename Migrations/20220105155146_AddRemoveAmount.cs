using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Migrations
{
    public partial class AddRemoveAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RemoveAmount",
                table: "BasketItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemoveAmount",
                table: "BasketItems");
        }
    }
}
