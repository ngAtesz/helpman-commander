using Microsoft.EntityFrameworkCore.Migrations;

namespace HelpmanCommander.Data.Migrations
{
    public partial class RemovedNavigationProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Stations_StationId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Competitions_CompetitionId",
                table: "Stations");

            migrationBuilder.AlterColumn<int>(
                name: "CompetitionId",
                table: "Stations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "StationId",
                table: "Exercises",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Stations_StationId",
                table: "Exercises",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Competitions_CompetitionId",
                table: "Stations",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Stations_StationId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Competitions_CompetitionId",
                table: "Stations");

            migrationBuilder.AlterColumn<int>(
                name: "CompetitionId",
                table: "Stations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StationId",
                table: "Exercises",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Stations_StationId",
                table: "Exercises",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Competitions_CompetitionId",
                table: "Stations",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
