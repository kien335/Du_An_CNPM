using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dự_Án_CNPM.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleTypeToParkingSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "ParkingSessions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "ParkingSessions");
        }
    }
}
