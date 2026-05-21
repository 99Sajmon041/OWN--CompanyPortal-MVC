using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyToDeviceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeprecated",
                table: "Devices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeprecated",
                table: "Devices");
        }
    }
}
