using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Seventeen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QaOptions_QuestionAndAnswers_QuestionAndAnswerId",
                schema: "dbo",
                table: "QaOptions");

            migrationBuilder.DropIndex(
                name: "IX_QaOptions_QuestionAndAnswerId",
                schema: "dbo",
                table: "QaOptions");

            migrationBuilder.DropColumn(
                name: "QuestionAndAnswerId",
                schema: "dbo",
                table: "QaOptions");

            migrationBuilder.CreateIndex(
                name: "IX_QaOptions_QaId",
                schema: "dbo",
                table: "QaOptions",
                column: "QaId");

            migrationBuilder.AddForeignKey(
                name: "FK_QaOptions_QuestionAndAnswers_QaId",
                schema: "dbo",
                table: "QaOptions",
                column: "QaId",
                principalSchema: "dbo",
                principalTable: "QuestionAndAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QaOptions_QuestionAndAnswers_QaId",
                schema: "dbo",
                table: "QaOptions");

            migrationBuilder.DropIndex(
                name: "IX_QaOptions_QaId",
                schema: "dbo",
                table: "QaOptions");

            migrationBuilder.AddColumn<int>(
                name: "QuestionAndAnswerId",
                schema: "dbo",
                table: "QaOptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QaOptions_QuestionAndAnswerId",
                schema: "dbo",
                table: "QaOptions",
                column: "QuestionAndAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_QaOptions_QuestionAndAnswers_QuestionAndAnswerId",
                schema: "dbo",
                table: "QaOptions",
                column: "QuestionAndAnswerId",
                principalSchema: "dbo",
                principalTable: "QuestionAndAnswers",
                principalColumn: "Id");
        }
    }
}
