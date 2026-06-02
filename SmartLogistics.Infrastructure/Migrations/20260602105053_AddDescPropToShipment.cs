using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLogistics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDescPropToShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Shipment",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Shipment");
        }
    }
}
