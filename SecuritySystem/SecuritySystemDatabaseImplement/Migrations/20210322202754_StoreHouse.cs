using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SecuritySystemDatabaseImplement.Migrations
{
    public partial class StoreHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreHouses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreHouseName = table.Column<string>(nullable: false),
                    ResponsiblePersonFCS = table.Column<string>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreHouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreHouseComponents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreHouseId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreHouseComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreHouseComponents_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreHouseComponents_StoreHouses_StoreHouseId",
                        column: x => x.StoreHouseId,
                        principalTable: "StoreHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreHouseComponents_ComponentId",
                table: "StoreHouseComponents",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreHouseComponents_StoreHouseId",
                table: "StoreHouseComponents",
                column: "StoreHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreHouseComponents");

            migrationBuilder.DropTable(
                name: "StoreHouses");
        }
    }
}
