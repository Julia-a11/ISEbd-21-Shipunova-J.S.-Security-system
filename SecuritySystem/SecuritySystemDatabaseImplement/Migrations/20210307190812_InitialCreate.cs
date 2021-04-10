using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SecuritySystemDatabaseImplement.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Secures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecureName = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecureId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Sum = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateImplement = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Secures_SecureId",
                        column: x => x.SecureId,
                        principalTable: "Secures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecureComponents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecureId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecureComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecureComponents_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecureComponents_Secures_SecureId",
                        column: x => x.SecureId,
                        principalTable: "Secures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SecureId",
                table: "Orders",
                column: "SecureId");

            migrationBuilder.CreateIndex(
                name: "IX_SecureComponents_ComponentId",
                table: "SecureComponents",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_SecureComponents_SecureId",
                table: "SecureComponents",
                column: "SecureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "SecureComponents");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Secures");
        }
    }
}
