using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class TwentySix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedById",
                schema: "dbo",
                table: "SeriesQuestionResponses");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                schema: "dbo",
                table: "SeriesQuestionResponses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "SeriesQuestionResponses");

            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                schema: "dbo",
                table: "CourseQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                schema: "dbo",
                table: "CourseQuestions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "CourseQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                schema: "dbo",
                table: "CourseQuestionOptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                schema: "dbo",
                table: "CourseQuestionOptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "CourseQuestionOptions",
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
                table: "CourseQuestions");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                schema: "dbo",
                table: "CourseQuestions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "CourseQuestions");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                schema: "dbo",
                table: "CourseQuestionOptions");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                schema: "dbo",
                table: "CourseQuestionOptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "CourseQuestionOptions");

            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                schema: "dbo",
                table: "SeriesQuestionResponses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                schema: "dbo",
                table: "SeriesQuestionResponses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "SeriesQuestionResponses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
