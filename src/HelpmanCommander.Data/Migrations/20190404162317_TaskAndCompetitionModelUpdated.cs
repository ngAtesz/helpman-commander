using Microsoft.EntityFrameworkCore.Migrations;

namespace HelpmanCommander.Data.Migrations
{
    public partial class TaskAndCompetitionModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Competitions_CompetitionId",
                table: "Stations");

            migrationBuilder.RenameColumn(
                name: "IsDefaultTask",
                table: "Tasks",
                newName: "IsDefault");

            migrationBuilder.AlterColumn<int>(
                name: "CompetitionId",
                table: "Stations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Finalized",
                table: "Competitions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Competitions_CompetitionId",
                table: "Stations",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Competitions_CompetitionId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "Finalized",
                table: "Competitions");

            migrationBuilder.RenameColumn(
                name: "IsDefault",
                table: "Tasks",
                newName: "IsDefaultTask");

            migrationBuilder.AlterColumn<int>(
                name: "CompetitionId",
                table: "Stations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Competitions_CompetitionId",
                table: "Stations",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
