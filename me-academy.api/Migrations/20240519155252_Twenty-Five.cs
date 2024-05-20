using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class TwentyFive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QaResponses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "QaOptions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "QuestionAndAnswers",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "CourseQuestions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsMultiple = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseQuestions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "dbo",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseQuestions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeriesQuestions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsMultiple = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeriesQuestions_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "dbo",
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesQuestions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeriesQuestions_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseQuestionOptions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseQuestionOptions_CourseQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "dbo",
                        principalTable: "CourseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesQuestionOptions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeriesQuestionOptions_SeriesQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "dbo",
                        principalTable: "SeriesQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseQuestionResponses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseQuestionResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseQuestionResponses_CourseQuestionOptions_OptionId",
                        column: x => x.OptionId,
                        principalSchema: "dbo",
                        principalTable: "CourseQuestionOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseQuestionResponses_CourseQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "dbo",
                        principalTable: "CourseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseQuestionResponses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "dbo",
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseQuestionResponses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeriesQuestionResponses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesQuestionResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeriesQuestionResponses_SeriesQuestionOptions_OptionId",
                        column: x => x.OptionId,
                        principalSchema: "dbo",
                        principalTable: "SeriesQuestionOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeriesQuestionResponses_SeriesQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "dbo",
                        principalTable: "SeriesQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesQuestionResponses_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "dbo",
                        principalTable: "Series",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeriesQuestionResponses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestionOptions_QuestionId",
                schema: "dbo",
                table: "CourseQuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestionResponses_CourseId",
                schema: "dbo",
                table: "CourseQuestionResponses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestionResponses_CreatedById",
                schema: "dbo",
                table: "CourseQuestionResponses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestionResponses_OptionId",
                schema: "dbo",
                table: "CourseQuestionResponses",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestionResponses_QuestionId",
                schema: "dbo",
                table: "CourseQuestionResponses",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestions_CourseId",
                schema: "dbo",
                table: "CourseQuestions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestions_CreatedById",
                schema: "dbo",
                table: "CourseQuestions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesQuestionOptions_QuestionId",
                schema: "dbo",
                table: "SeriesQuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesQuestionResponses_CreatedById",
                schema: "dbo",
                table: "SeriesQuestionResponses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesQuestionResponses_OptionId",
                schema: "dbo",
                table: "SeriesQuestionResponses",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesQuestionResponses_QuestionId",
                schema: "dbo",
                table: "SeriesQuestionResponses",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesQuestionResponses_SeriesId",
                schema: "dbo",
                table: "SeriesQuestionResponses",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesQuestions_CreatedById",
                schema: "dbo",
                table: "SeriesQuestions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesQuestions_SeriesId",
                schema: "dbo",
                table: "SeriesQuestions",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesQuestions_UpdatedById",
                schema: "dbo",
                table: "SeriesQuestions",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseQuestionResponses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SeriesQuestionResponses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CourseQuestionOptions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SeriesQuestionOptions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CourseQuestions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SeriesQuestions",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "QuestionAndAnswers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsMultipleChoice = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAndAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAndAnswers_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "dbo",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAndAnswers_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QaOptions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QaId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QaOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QaOptions_QuestionAndAnswers_QaId",
                        column: x => x.QaId,
                        principalSchema: "dbo",
                        principalTable: "QuestionAndAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QaResponses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QaResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QaResponses_QaOptions_OptionId",
                        column: x => x.OptionId,
                        principalSchema: "dbo",
                        principalTable: "QaOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QaResponses_QuestionAndAnswers_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "dbo",
                        principalTable: "QuestionAndAnswers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QaResponses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QaOptions_QaId",
                schema: "dbo",
                table: "QaOptions",
                column: "QaId");

            migrationBuilder.CreateIndex(
                name: "IX_QaResponses_CreatedById",
                schema: "dbo",
                table: "QaResponses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QaResponses_OptionId",
                schema: "dbo",
                table: "QaResponses",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_QaResponses_QuestionId",
                schema: "dbo",
                table: "QaResponses",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAndAnswers_CourseId",
                schema: "dbo",
                table: "QuestionAndAnswers",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAndAnswers_CreatedById",
                schema: "dbo",
                table: "QuestionAndAnswers",
                column: "CreatedById");
        }
    }
}
