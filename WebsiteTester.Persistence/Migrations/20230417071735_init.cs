using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteTester.Persistenсe.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestedLink",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestedLink", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinkTestResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    RenderTimeMilliseconds = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInSitemap = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsInWebsite = table.Column<bool>(type: "INTEGER", nullable: false),
                    TestedLinkId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkTestResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkTestResult_TestedLink_TestedLinkId",
                        column: x => x.TestedLinkId,
                        principalTable: "TestedLink",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkTestResult_TestedLinkId",
                table: "LinkTestResult",
                column: "TestedLinkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkTestResult");

            migrationBuilder.DropTable(
                name: "TestedLink");
        }
    }
}
