using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dự_Án_CNPM.Migrations
{
    /// <inheritdoc />
    public partial class AddCardSeedingAndLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "CardId", "Note", "Status" },
                values: new object[,]
                {
                    { "C01", "Thẻ thử nghiệm", "Available" },
                    { "C02", "Thẻ thử nghiệm", "Available" },
                    { "C03", "Thẻ thử nghiệm", "Available" },
                    { "C04", "Thẻ thử nghiệm", "Available" },
                    { "C05", "Thẻ thử nghiệm", "Available" },
                    { "C06", "Thẻ thử nghiệm", "Available" },
                    { "C07", "Thẻ thử nghiệm", "Available" },
                    { "C08", "Thẻ thử nghiệm", "Available" },
                    { "C09", "Thẻ thử nghiệm", "Available" },
                    { "C10", "Thẻ thử nghiệm", "Available" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardId",
                keyValue: "C01");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardId",
                keyValue: "C02");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardId",
                keyValue: "C03");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardId",
                keyValue: "C04");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardId",
                keyValue: "C05");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardId",
                keyValue: "C06");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardId",
                keyValue: "C07");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardId",
                keyValue: "C08");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardId",
                keyValue: "C09");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardId",
                keyValue: "C10");
        }
    }
}
