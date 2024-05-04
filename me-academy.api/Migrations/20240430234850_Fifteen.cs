using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Fifteen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionAndAnswer",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsMultipleChoice = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAndAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAndAnswer_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "dbo",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAndAnswer_Users_CreatedById",
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
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    QuestionAndAnswerId = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QaOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QaOptions_QuestionAndAnswer_QuestionAndAnswerId",
                        column: x => x.QuestionAndAnswerId,
                        principalSchema: "dbo",
                        principalTable: "QuestionAndAnswer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QaResponses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QaId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        name: "FK_QaResponses_QuestionAndAnswer_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "dbo",
                        principalTable: "QuestionAndAnswer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QaResponses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QaOptions_QuestionAndAnswerId",
                schema: "dbo",
                table: "QaOptions",
                column: "QuestionAndAnswerId");

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
                name: "IX_QuestionAndAnswer_CourseId",
                schema: "dbo",
                table: "QuestionAndAnswer",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAndAnswer_CreatedById",
                schema: "dbo",
                table: "QuestionAndAnswer",
                column: "CreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QaResponses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "QaOptions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "QuestionAndAnswer",
                schema: "dbo");
        }
    }
}
