using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class ThirtyOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                schema: "dbo",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "TotalLeft",
                schema: "dbo",
                table: "Discounts",
                newName: "TotalUsed");

            migrationBuilder.AddColumn<int>(
                name: "TotalAvailable",
                schema: "dbo",
                table: "Discounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAvailable",
                schema: "dbo",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "TotalUsed",
                schema: "dbo",
                table: "Discounts",
                newName: "TotalLeft");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                schema: "dbo",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
