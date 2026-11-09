using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatML.Migrations
{
    /// <inheritdoc />
    public partial class addCreatorEditorToGOther : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Utopias",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedId",
                table: "Utopias",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EditorId",
                table: "Utopias",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Utopias",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Tournaments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedId",
                table: "Tournaments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EditorId",
                table: "Tournaments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tournaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Players",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedId",
                table: "Players",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EditorId",
                table: "Players",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Utopias_CreatorId",
                table: "Utopias",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Utopias_EditorId",
                table: "Utopias",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_CreatorId",
                table: "Tournaments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_EditorId",
                table: "Tournaments",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CreatorId",
                table: "Players",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_EditorId",
                table: "Players",
                column: "EditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Logins_CreatorId",
                table: "Players",
                column: "CreatorId",
                principalTable: "Logins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Logins_EditorId",
                table: "Players",
                column: "EditorId",
                principalTable: "Logins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Logins_CreatorId",
                table: "Tournaments",
                column: "CreatorId",
                principalTable: "Logins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Logins_EditorId",
                table: "Tournaments",
                column: "EditorId",
                principalTable: "Logins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utopias_Logins_CreatorId",
                table: "Utopias",
                column: "CreatorId",
                principalTable: "Logins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utopias_Logins_EditorId",
                table: "Utopias",
                column: "EditorId",
                principalTable: "Logins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Logins_CreatorId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Logins_EditorId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Logins_CreatorId",
                table: "Tournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Logins_EditorId",
                table: "Tournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Utopias_Logins_CreatorId",
                table: "Utopias");

            migrationBuilder.DropForeignKey(
                name: "FK_Utopias_Logins_EditorId",
                table: "Utopias");

            migrationBuilder.DropIndex(
                name: "IX_Utopias_CreatorId",
                table: "Utopias");

            migrationBuilder.DropIndex(
                name: "IX_Utopias_EditorId",
                table: "Utopias");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_CreatorId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_EditorId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Players_CreatorId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_EditorId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Utopias");

            migrationBuilder.DropColumn(
                name: "DeletedId",
                table: "Utopias");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "Utopias");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Utopias");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "DeletedId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "DeletedId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Players");
        }
    }
}
