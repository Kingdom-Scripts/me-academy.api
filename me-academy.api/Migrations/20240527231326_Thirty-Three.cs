using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class ThirtyThree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                schema: "dbo",
                table: "CourseQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOnUtc",
                schema: "dbo",
                table: "CourseQuestions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                schema: "dbo",
                table: "CourseQuestionOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                schema: "dbo",
                table: "CourseQuestionOptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOnUtc",
                schema: "dbo",
                table: "CourseQuestionOptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestions_UpdatedById",
                schema: "dbo",
                table: "CourseQuestions",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseQuestions_Users_UpdatedById",
                schema: "dbo",
                table: "CourseQuestions",
                column: "UpdatedById",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseQuestions_Users_UpdatedById",
                schema: "dbo",
                table: "CourseQuestions");

            migrationBuilder.DropIndex(
                name: "IX_CourseQuestions_UpdatedById",
                schema: "dbo",
                table: "CourseQuestions");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                schema: "dbo",
                table: "CourseQuestions");

            migrationBuilder.DropColumn(
                name: "UpdatedOnUtc",
                schema: "dbo",
                table: "CourseQuestions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "dbo",
                table: "CourseQuestionOptions");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                schema: "dbo",
                table: "CourseQuestionOptions");

            migrationBuilder.DropColumn(
                name: "UpdatedOnUtc",
                schema: "dbo",
                table: "CourseQuestionOptions");
        }
    }
}
