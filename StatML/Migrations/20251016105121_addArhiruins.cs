using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatML.Migrations
{
    /// <inheritdoc />
    public partial class addArhiruins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArhiRuin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateOnly = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    BlackGemCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatorId = table.Column<int>(type: "INTEGER", nullable: true),
                    EditorId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArhiRuin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArhiRuin_Logins_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Logins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArhiRuin_Logins_EditorId",
                        column: x => x.EditorId,
                        principalTable: "Logins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArhiRuin_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArhiRuin_CreatorId",
                table: "ArhiRuin",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArhiRuin_EditorId",
                table: "ArhiRuin",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArhiRuin_PlayerId",
                table: "ArhiRuin",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArhiRuin");
        }
    }
}
