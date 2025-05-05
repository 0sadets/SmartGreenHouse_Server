using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartGreenhouse.Migrations
{
    /// <inheritdoc />
    public partial class SeedPlants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plants",
                columns: new[] { "Id", "Category", "ExampleNames", "Features", "OptimalAirHumidityMax", "OptimalAirHumidityMin", "OptimalAirTempMax", "OptimalAirTempMin", "OptimalLightHourPerDay", "OptimalLightMax", "OptimalLightMin", "OptimalSoilHumidityMax", "OptimalSoilHumidityMin", "OptimalSoilTempMax", "OptimalSoilTempMin" },
                values: new object[,]
                {
                    { 1, "Овочі", "Помідори, Огірки, Перець, Баклажани", "Огірки потребують більшої вологості, баклажани — стабільного тепла, перець — чутливий до перепадів температур.", 80f, 60f, 26f, 18f, 10f, 30000f, 18000f, 75f, 65f, 24f, 18f },
                    { 2, "Зелень", "Салат, Шпинат, Кріп, Петрушка, Базилік", "Базилік полюбляє тепліші умови, ніж інша зелень.", 90f, 70f, 22f, 15f, 8f, 20000f, 10000f, 70f, 60f, 20f, 16f },
                    { 3, "Коренеплоди", "Морква, Буряк, Редис, Картопля", "Редис швидко росте в прохолодних умовах, картопля потребує глибокого ґрунту.", 75f, 60f, 20f, 12f, 9f, 25000f, 15000f, 80f, 65f, 18f, 10f },
                    { 4, "Ягоди", "Полуниця, Малина, Лохина, Смородина", "Лохина потребує кислий ґрунт; малина не переносить застійної води.", 85f, 60f, 24f, 16f, 8f, 30000f, 20000f, 80f, 70f, 20f, 15f },
                    { 5, "Фрукти", "Лимон, Апельсин, Гранат, Інжир", "Цитрусові не переносять переохолодження; інжир потребує періоду спокою взимку.", 70f, 50f, 28f, 20f, 10f, 35000f, 25000f, 75f, 60f, 22f, 18f },
                    { 6, "Тепличні квіти", "Троянди, Орхідеї, Тюльпани", "Орхідеї потребують розсіяного світла; тюльпани люблять прохолодні ночі.", 80f, 60f, 25f, 18f, 8f, 25000f, 12000f, 70f, 55f, 22f, 16f },
                    { 7, "Гриби", "Печериці, Гливи, Трюфелі", "Гриби не потребують багато світла; трюфелі ростуть у симбіозі з деревами.", 95f, 80f, 20f, 14f, 2f, 5000f, 1000f, 90f, 75f, 18f, 14f }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
