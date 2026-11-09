using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatML.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatingArmy_Players_PlayerId",
                table: "RatingArmy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatingArmy",
                table: "RatingArmy");

            migrationBuilder.RenameTable(
                name: "RatingArmy",
                newName: "RatingArmies");

            migrationBuilder.RenameIndex(
                name: "IX_RatingArmy_PlayerId",
                table: "RatingArmies",
                newName: "IX_RatingArmies_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatingArmies",
                table: "RatingArmies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RatingArmies_Players_PlayerId",
                table: "RatingArmies",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatingArmies_Players_PlayerId",
                table: "RatingArmies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatingArmies",
                table: "RatingArmies");

            migrationBuilder.RenameTable(
                name: "RatingArmies",
                newName: "RatingArmy");

            migrationBuilder.RenameIndex(
                name: "IX_RatingArmies_PlayerId",
                table: "RatingArmy",
                newName: "IX_RatingArmy_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatingArmy",
                table: "RatingArmy",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RatingArmy_Players_PlayerId",
                table: "RatingArmy",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
