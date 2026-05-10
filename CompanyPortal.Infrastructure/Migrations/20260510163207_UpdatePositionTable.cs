
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePositionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HourlyRate",
                table: "Positions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "AccessValidUntil",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "AccessValidUntil",
                table: "AspNetUsers");
        }
    }
}
