using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatML.Migrations
{
    /// <inheritdoc />
    public partial class RecreateFlagTournamentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE TABLE FlagTournamentDetails (
                Id INTEGER NOT NULL CONSTRAINT PK_FlagTournamentDetails PRIMARY KEY AUTOINCREMENT,
                TournamentDate TEXT NOT NULL,
                PlayerId INTEGER NOT NULL,
                CountOfBasicFlags INTEGER NULL,
                CountOfOtherFlags INTEGER NULL,
                CountOfNotGivenFlags INTEGER NULL,
                CONSTRAINT FK_FlagTournamentDetails_Players_PlayerId FOREIGN KEY (PlayerId) REFERENCES Players (Id) ON DELETE CASCADE
            )");

            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS IX_FlagTournamentDetails_PlayerId ON FlagTournamentDetails (PlayerId)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE FlagTournamentDetails");
        }
    }
}
