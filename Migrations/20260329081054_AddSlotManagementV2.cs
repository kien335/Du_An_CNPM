using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dự_Án_CNPM.Migrations
{
    /// <inheritdoc />
    public partial class AddSlotManagementV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SlotId",
                table: "ParkingSessions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.InsertData(
                table: "ParkingSlots",
                columns: new[] { "SlotId", "Status" },
                values: new object[,]
                {
                    { "A01", "Empty" },
                    { "A02", "Empty" },
                    { "A03", "Empty" },
                    { "A04", "Empty" },
                    { "A05", "Empty" },
                    { "A06", "Empty" },
                    { "A07", "Empty" },
                    { "A08", "Empty" },
                    { "A09", "Empty" },
                    { "A10", "Empty" },
                    { "B01", "Empty" },
                    { "B02", "Empty" },
                    { "B03", "Empty" },
                    { "B04", "Empty" },
                    { "B05", "Empty" },
                    { "B06", "Empty" },
                    { "B07", "Empty" },
                    { "B08", "Empty" },
                    { "B09", "Empty" },
                    { "B10", "Empty" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "A01");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "A02");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "A03");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "A04");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "A05");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "A06");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "A07");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "A08");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "A09");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "A10");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "B01");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "B02");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "B03");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "B04");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "B05");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "B06");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "B07");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "B08");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "B09");

            migrationBuilder.DeleteData(
                table: "ParkingSlots",
                keyColumn: "SlotId",
                keyValue: "B10");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "ParkingSessions");
        }
    }
}
