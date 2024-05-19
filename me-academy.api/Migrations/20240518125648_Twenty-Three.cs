using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class TwentyThree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesCourses_Series_SeriesId1",
                schema: "dbo",
                table: "SeriesCourses");

            migrationBuilder.DropIndex(
                name: "IX_SeriesCourses_SeriesId1",
                schema: "dbo",
                table: "SeriesCourses");

            migrationBuilder.DropColumn(
                name: "SeriesId1",
                schema: "dbo",
                table: "SeriesCourses");

            migrationBuilder.CreateTable(
                name: "SmeHubs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uid = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmeHubs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmeHubs_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "dbo",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmeHubs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmeHubs_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmeHubs_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmeHubs_CreatedById",
                schema: "dbo",
                table: "SmeHubs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmeHubs_DeletedById",
                schema: "dbo",
                table: "SmeHubs",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmeHubs_DocumentId",
                schema: "dbo",
                table: "SmeHubs",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SmeHubs_UpdatedById",
                schema: "dbo",
                table: "SmeHubs",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmeHubs",
                schema: "dbo");

            migrationBuilder.AddColumn<int>(
                name: "SeriesId1",
                schema: "dbo",
                table: "SeriesCourses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeriesCourses_SeriesId1",
                schema: "dbo",
                table: "SeriesCourses",
                column: "SeriesId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesCourses_Series_SeriesId1",
                schema: "dbo",
                table: "SeriesCourses",
                column: "SeriesId1",
                principalSchema: "dbo",
                principalTable: "Series",
                principalColumn: "Id");
        }
    }
}
