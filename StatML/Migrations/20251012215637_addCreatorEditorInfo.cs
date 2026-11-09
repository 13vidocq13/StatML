using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatML.Migrations
{
    /// <inheritdoc />
    public partial class addCreatorEditorInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Salts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedId",
                table: "Salts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EditorId",
                table: "Salts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Salts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Salts");

            migrationBuilder.DropColumn(
                name: "DeletedId",
                table: "Salts");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "Salts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Salts");
        }
    }
}
