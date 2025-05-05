using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartGreenhouse.Migrations
{
    /// <inheritdoc />
    public partial class ImprovedPlants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "OptimalLightHourPerDay",
                table: "Plants",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OptimalSoilTempMax",
                table: "Plants",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OptimalSoilTempMin",
                table: "Plants",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptimalLightHourPerDay",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "OptimalSoilTempMax",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "OptimalSoilTempMin",
                table: "Plants");
        }
    }
}
