using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatML.Migrations
{
    /// <inheritdoc />
    public partial class fixArhi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArhiRuin_Logins_CreatorId",
                table: "ArhiRuin");

            migrationBuilder.DropForeignKey(
                name: "FK_ArhiRuin_Logins_EditorId",
                table: "ArhiRuin");

            migrationBuilder.DropForeignKey(
                name: "FK_ArhiRuin_Players_PlayerId",
                table: "ArhiRuin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArhiRuin",
                table: "ArhiRuin");

            migrationBuilder.RenameTable(
                name: "ArhiRuin",
                newName: "ArhiRuins");

            migrationBuilder.RenameIndex(
                name: "IX_ArhiRuin_PlayerId",
                table: "ArhiRuins",
                newName: "IX_ArhiRuins_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_ArhiRuin_EditorId",
                table: "ArhiRuins",
                newName: "IX_ArhiRuins_EditorId");

            migrationBuilder.RenameIndex(
                name: "IX_ArhiRuin_CreatorId",
                table: "ArhiRuins",
                newName: "IX_ArhiRuins_CreatorId");

            migrationBuilder.AddColumn<int>(
                name: "HorizontalPosition",
                table: "ArhiRuins",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VerticalPosition",
                table: "ArhiRuins",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArhiRuins",
                table: "ArhiRuins",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArhiRuins_Logins_CreatorId",
                table: "ArhiRuins",
                column: "CreatorId",
                principalTable: "Logins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArhiRuins_Logins_EditorId",
                table: "ArhiRuins",
                column: "EditorId",
                principalTable: "Logins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArhiRuins_Players_PlayerId",
                table: "ArhiRuins",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArhiRuins_Logins_CreatorId",
                table: "ArhiRuins");

            migrationBuilder.DropForeignKey(
                name: "FK_ArhiRuins_Logins_EditorId",
                table: "ArhiRuins");

            migrationBuilder.DropForeignKey(
                name: "FK_ArhiRuins_Players_PlayerId",
                table: "ArhiRuins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArhiRuins",
                table: "ArhiRuins");

            migrationBuilder.DropColumn(
                name: "HorizontalPosition",
                table: "ArhiRuins");

            migrationBuilder.DropColumn(
                name: "VerticalPosition",
                table: "ArhiRuins");

            migrationBuilder.RenameTable(
                name: "ArhiRuins",
                newName: "ArhiRuin");

            migrationBuilder.RenameIndex(
                name: "IX_ArhiRuins_PlayerId",
                table: "ArhiRuin",
                newName: "IX_ArhiRuin_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_ArhiRuins_EditorId",
                table: "ArhiRuin",
                newName: "IX_ArhiRuin_EditorId");

            migrationBuilder.RenameIndex(
                name: "IX_ArhiRuins_CreatorId",
                table: "ArhiRuin",
                newName: "IX_ArhiRuin_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArhiRuin",
                table: "ArhiRuin",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArhiRuin_Logins_CreatorId",
                table: "ArhiRuin",
                column: "CreatorId",
                principalTable: "Logins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArhiRuin_Logins_EditorId",
                table: "ArhiRuin",
                column: "EditorId",
                principalTable: "Logins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArhiRuin_Players_PlayerId",
                table: "ArhiRuin",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
