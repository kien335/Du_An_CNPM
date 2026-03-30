using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dự_Án_CNPM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateParkingFeeLogicV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlockRate",
                table: "PricingRules",
                newName: "BasePrice");

            migrationBuilder.RenameColumn(
                name: "TotalFee",
                table: "ParkingSessions",
                newName: "TotalPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BasePrice",
                table: "PricingRules",
                newName: "BlockRate");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "ParkingSessions",
                newName: "TotalFee");
        }
    }
}
