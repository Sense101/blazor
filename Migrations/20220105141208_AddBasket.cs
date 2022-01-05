using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Migrations
{
    public partial class AddBasket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BasketId",
                table: "BasketItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Baskets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_BasketId",
                table: "BasketItems",
                column: "BasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Baskets_BasketId",
                table: "BasketItems",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Baskets_BasketId",
                table: "BasketItems");

            migrationBuilder.DropTable(
                name: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_BasketItems_BasketId",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "BasketItems");
        }
    }
}
