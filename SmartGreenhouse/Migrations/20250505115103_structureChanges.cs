using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartGreenhouse.Migrations
{
    /// <inheritdoc />
    public partial class structureChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GreenhousePlants");

            migrationBuilder.RenameColumn(
                name: "TargetSoilMoisture",
                table: "UserSettings",
                newName: "SoilTempMin");

            migrationBuilder.RenameColumn(
                name: "TargetLight",
                table: "UserSettings",
                newName: "SoilTempMax");

            migrationBuilder.RenameColumn(
                name: "TargetAirTemp",
                table: "UserSettings",
                newName: "SoilHumidityMin");

            migrationBuilder.RenameColumn(
                name: "TargetAirHumidity",
                table: "UserSettings",
                newName: "SoilHumidityMax");

            migrationBuilder.AddColumn<float>(
                name: "AirHumidityMax",
                table: "UserSettings",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "AirHumidityMin",
                table: "UserSettings",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "AirTempMax",
                table: "UserSettings",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "AirTempMin",
                table: "UserSettings",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "LightHoursPerDay",
                table: "UserSettings",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "LightMax",
                table: "UserSettings",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "LightMin",
                table: "UserSettings",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "GreenhousePlant",
                columns: table => new
                {
                    GreenhousesId = table.Column<int>(type: "int", nullable: false),
                    PlantsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreenhousePlant", x => new { x.GreenhousesId, x.PlantsId });
                    table.ForeignKey(
                        name: "FK_GreenhousePlant_Greenhouses_GreenhousesId",
                        column: x => x.GreenhousesId,
                        principalTable: "Greenhouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GreenhousePlant_Plants_PlantsId",
                        column: x => x.PlantsId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GreenhousePlant_PlantsId",
                table: "GreenhousePlant",
                column: "PlantsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GreenhousePlant");

            migrationBuilder.DropColumn(
                name: "AirHumidityMax",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "AirHumidityMin",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "AirTempMax",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "AirTempMin",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "LightHoursPerDay",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "LightMax",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "LightMin",
                table: "UserSettings");

            migrationBuilder.RenameColumn(
                name: "SoilTempMin",
                table: "UserSettings",
                newName: "TargetSoilMoisture");

            migrationBuilder.RenameColumn(
                name: "SoilTempMax",
                table: "UserSettings",
                newName: "TargetLight");

            migrationBuilder.RenameColumn(
                name: "SoilHumidityMin",
                table: "UserSettings",
                newName: "TargetAirTemp");

            migrationBuilder.RenameColumn(
                name: "SoilHumidityMax",
                table: "UserSettings",
                newName: "TargetAirHumidity");

            migrationBuilder.CreateTable(
                name: "GreenhousePlants",
                columns: table => new
                {
                    GreenhouseId = table.Column<int>(type: "int", nullable: false),
                    PlantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreenhousePlants", x => new { x.GreenhouseId, x.PlantId });
                    table.ForeignKey(
                        name: "FK_GreenhousePlants_Greenhouses_GreenhouseId",
                        column: x => x.GreenhouseId,
                        principalTable: "Greenhouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GreenhousePlants_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GreenhousePlants_PlantId",
                table: "GreenhousePlants",
                column: "PlantId");
        }
    }
}
