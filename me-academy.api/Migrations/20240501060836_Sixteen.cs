using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Sixteen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QaOptions_QuestionAndAnswer_QuestionAndAnswerId",
                schema: "dbo",
                table: "QaOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_QaResponses_QuestionAndAnswer_QuestionId",
                schema: "dbo",
                table: "QaResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAndAnswer_Courses_CourseId",
                schema: "dbo",
                table: "QuestionAndAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAndAnswer_Users_CreatedById",
                schema: "dbo",
                table: "QuestionAndAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionAndAnswer",
                schema: "dbo",
                table: "QuestionAndAnswer");

            migrationBuilder.RenameTable(
                name: "QuestionAndAnswer",
                schema: "dbo",
                newName: "QuestionAndAnswers",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAndAnswer_CreatedById",
                schema: "dbo",
                table: "QuestionAndAnswers",
                newName: "IX_QuestionAndAnswers_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAndAnswer_CourseId",
                schema: "dbo",
                table: "QuestionAndAnswers",
                newName: "IX_QuestionAndAnswers_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionAndAnswers",
                schema: "dbo",
                table: "QuestionAndAnswers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QaOptions_QuestionAndAnswers_QuestionAndAnswerId",
                schema: "dbo",
                table: "QaOptions",
                column: "QuestionAndAnswerId",
                principalSchema: "dbo",
                principalTable: "QuestionAndAnswers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QaResponses_QuestionAndAnswers_QuestionId",
                schema: "dbo",
                table: "QaResponses",
                column: "QuestionId",
                principalSchema: "dbo",
                principalTable: "QuestionAndAnswers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAndAnswers_Courses_CourseId",
                schema: "dbo",
                table: "QuestionAndAnswers",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAndAnswers_Users_CreatedById",
                schema: "dbo",
                table: "QuestionAndAnswers",
                column: "CreatedById",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QaOptions_QuestionAndAnswers_QuestionAndAnswerId",
                schema: "dbo",
                table: "QaOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_QaResponses_QuestionAndAnswers_QuestionId",
                schema: "dbo",
                table: "QaResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAndAnswers_Courses_CourseId",
                schema: "dbo",
                table: "QuestionAndAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAndAnswers_Users_CreatedById",
                schema: "dbo",
                table: "QuestionAndAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionAndAnswers",
                schema: "dbo",
                table: "QuestionAndAnswers");

            migrationBuilder.RenameTable(
                name: "QuestionAndAnswers",
                schema: "dbo",
                newName: "QuestionAndAnswer",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAndAnswers_CreatedById",
                schema: "dbo",
                table: "QuestionAndAnswer",
                newName: "IX_QuestionAndAnswer_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAndAnswers_CourseId",
                schema: "dbo",
                table: "QuestionAndAnswer",
                newName: "IX_QuestionAndAnswer_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionAndAnswer",
                schema: "dbo",
                table: "QuestionAndAnswer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QaOptions_QuestionAndAnswer_QuestionAndAnswerId",
                schema: "dbo",
                table: "QaOptions",
                column: "QuestionAndAnswerId",
                principalSchema: "dbo",
                principalTable: "QuestionAndAnswer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QaResponses_QuestionAndAnswer_QuestionId",
                schema: "dbo",
                table: "QaResponses",
                column: "QuestionId",
                principalSchema: "dbo",
                principalTable: "QuestionAndAnswer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAndAnswer_Courses_CourseId",
                schema: "dbo",
                table: "QuestionAndAnswer",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAndAnswer_Users_CreatedById",
                schema: "dbo",
                table: "QuestionAndAnswer",
                column: "CreatedById",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
