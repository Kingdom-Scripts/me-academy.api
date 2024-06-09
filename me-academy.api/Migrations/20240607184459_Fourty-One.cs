using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class FourtyOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesQuestionResponses_Series_SeriesId",
                schema: "dbo",
                table: "SeriesQuestionResponses");

            migrationBuilder.DropIndex(
                name: "IX_SeriesQuestionResponses_SeriesId",
                schema: "dbo",
                table: "SeriesQuestionResponses");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                schema: "dbo",
                table: "SeriesQuestionResponses");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                schema: "dbo",
                table: "SeriesQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SeriesQuestions_CourseId",
                schema: "dbo",
                table: "SeriesQuestions",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesQuestions_Courses_CourseId",
                schema: "dbo",
                table: "SeriesQuestions",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesQuestions_Courses_CourseId",
                schema: "dbo",
                table: "SeriesQuestions");

            migrationBuilder.DropIndex(
                name: "IX_SeriesQuestions_CourseId",
                schema: "dbo",
                table: "SeriesQuestions");

            migrationBuilder.DropColumn(
                name: "CourseId",
                schema: "dbo",
                table: "SeriesQuestions");

            migrationBuilder.AddColumn<int>(
                name: "SeriesId",
                schema: "dbo",
                table: "SeriesQuestionResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SeriesQuestionResponses_SeriesId",
                schema: "dbo",
                table: "SeriesQuestionResponses",
                column: "SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesQuestionResponses_Series_SeriesId",
                schema: "dbo",
                table: "SeriesQuestionResponses",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "Series",
                principalColumn: "Id");
        }
    }
}
