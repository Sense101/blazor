using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "baskets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baskets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    BasketID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_items_baskets_BasketID",
                        column: x => x.BasketID,
                        principalTable: "baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_items_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "baskets",
                columns: new[] { "Id", "LastUpdated" },
                values: new object[] { 1, new DateTime(2022, 1, 3, 15, 53, 50, 924, DateTimeKind.Local).AddTicks(2922) });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "Active", "Description", "Name", "Price" },
                values: new object[] { 1, true, "Description", "Product 1", 2.5m });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "Active", "Description", "Name", "Price" },
                values: new object[] { 2, true, "Description", "Product 2", 8.5m });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "Active", "Description", "Name", "Price" },
                values: new object[] { 3, true, "Description", "Product 3", 6m });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "Active", "Description", "Name", "Price" },
                values: new object[] { 4, true, "Description", "Product 4", 2.6m });

            migrationBuilder.CreateIndex(
                name: "IX_items_BasketID",
                table: "items",
                column: "BasketID");

            migrationBuilder.CreateIndex(
                name: "IX_items_ProductId",
                table: "items",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "baskets");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
