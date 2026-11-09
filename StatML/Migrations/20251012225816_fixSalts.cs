using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatML.Migrations
{
    /// <inheritdoc />
    public partial class fixSalts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Salts_CreatorId",
                table: "Salts",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Salts_EditorId",
                table: "Salts",
                column: "EditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Salts_Logins_CreatorId",
                table: "Salts",
                column: "CreatorId",
                principalTable: "Logins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Salts_Logins_EditorId",
                table: "Salts",
                column: "EditorId",
                principalTable: "Logins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salts_Logins_CreatorId",
                table: "Salts");

            migrationBuilder.DropForeignKey(
                name: "FK_Salts_Logins_EditorId",
                table: "Salts");

            migrationBuilder.DropIndex(
                name: "IX_Salts_CreatorId",
                table: "Salts");

            migrationBuilder.DropIndex(
                name: "IX_Salts_EditorId",
                table: "Salts");
        }
    }
}
