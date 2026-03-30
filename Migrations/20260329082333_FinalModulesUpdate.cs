using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dự_Án_CNPM.Migrations
{
    /// <inheritdoc />
    public partial class FinalModulesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Cards");
        }
    }
}
