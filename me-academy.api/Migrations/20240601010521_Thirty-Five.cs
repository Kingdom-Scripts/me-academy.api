using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class ThirtyFive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAtUtc",
                schema: "dbo",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "ExpiresAtUtc",
                schema: "dbo",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "PurchasedAtUtc",
                schema: "dbo",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "UserContentId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                schema: "dbo",
                table: "UserCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpired",
                schema: "dbo",
                table: "UserCourses");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAtUtc",
                schema: "dbo",
                table: "UserCourses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAtUtc",
                schema: "dbo",
                table: "UserCourses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchasedAtUtc",
                schema: "dbo",
                table: "UserCourses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserContentId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: true);
        }
    }
}
