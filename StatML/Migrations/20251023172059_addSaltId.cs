using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatML.Migrations
{
    /// <inheritdoc />
    public partial class addSaltId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaltId",
                table: "Salts",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaltId",
                table: "Salts");
        }
    }
}
