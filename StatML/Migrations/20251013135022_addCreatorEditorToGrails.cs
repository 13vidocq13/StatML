using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatML.Migrations
{
    /// <inheritdoc />
    public partial class addCreatorEditorToGrails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Grails",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedId",
                table: "Grails",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EditorId",
                table: "Grails",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Grails",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Grails_CreatorId",
                table: "Grails",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Grails_EditorId",
                table: "Grails",
                column: "EditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grails_Logins_CreatorId",
                table: "Grails",
                column: "CreatorId",
                principalTable: "Logins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grails_Logins_EditorId",
                table: "Grails",
                column: "EditorId",
                principalTable: "Logins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grails_Logins_CreatorId",
                table: "Grails");

            migrationBuilder.DropForeignKey(
                name: "FK_Grails_Logins_EditorId",
                table: "Grails");

            migrationBuilder.DropIndex(
                name: "IX_Grails_CreatorId",
                table: "Grails");

            migrationBuilder.DropIndex(
                name: "IX_Grails_EditorId",
                table: "Grails");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Grails");

            migrationBuilder.DropColumn(
                name: "DeletedId",
                table: "Grails");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "Grails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Grails");
        }
    }
}
