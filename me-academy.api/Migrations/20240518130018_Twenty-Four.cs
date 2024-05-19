using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class TwentyFour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                schema: "dbo",
                table: "SeriesCourses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                schema: "dbo",
                table: "SeriesCourses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "SeriesCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedById",
                schema: "dbo",
                table: "SeriesCourses");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                schema: "dbo",
                table: "SeriesCourses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "SeriesCourses");
        }
    }
}
