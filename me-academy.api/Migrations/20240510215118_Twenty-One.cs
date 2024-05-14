using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class TwentyOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Courses",
                type: "nvarchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)");

            migrationBuilder.AddColumn<bool>(
                name: "ForSeriesOnly",
                schema: "dbo",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Series",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uid = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedById = table.Column<int>(type: "int", nullable: true),
                    PublishedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Series_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Series_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Series_Users_PublishedById",
                        column: x => x.PublishedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Series_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeriesAuditLogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesAuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeriesAuditLogs_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "dbo",
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesAuditLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeriesCourses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SeriesId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeriesCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "dbo",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesCourses_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "dbo",
                        principalTable: "Series",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeriesCourses_Series_SeriesId1",
                        column: x => x.SeriesId1,
                        principalSchema: "dbo",
                        principalTable: "Series",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeriesCourses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeriesCourses_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeriesPreviews",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    UploadToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VideoDuration = table.Column<int>(type: "int", nullable: false),
                    IsUploaded = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesPreviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeriesPreviews_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "dbo",
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesPrices",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    DurationId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeriesPrices_Durations_DurationId",
                        column: x => x.DurationId,
                        principalSchema: "dbo",
                        principalTable: "Durations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesPrices_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "dbo",
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Series_CreatedById",
                schema: "dbo",
                table: "Series",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Series_DeletedById",
                schema: "dbo",
                table: "Series",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Series_PublishedById",
                schema: "dbo",
                table: "Series",
                column: "PublishedById");

            migrationBuilder.CreateIndex(
                name: "IX_Series_UpdatedById",
                schema: "dbo",
                table: "Series",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesAuditLogs_CreatedById",
                schema: "dbo",
                table: "SeriesAuditLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesAuditLogs_SeriesId",
                schema: "dbo",
                table: "SeriesAuditLogs",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesCourses_CourseId",
                schema: "dbo",
                table: "SeriesCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesCourses_CreatedById",
                schema: "dbo",
                table: "SeriesCourses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesCourses_SeriesId",
                schema: "dbo",
                table: "SeriesCourses",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesCourses_SeriesId1",
                schema: "dbo",
                table: "SeriesCourses",
                column: "SeriesId1");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesCourses_UpdatedById",
                schema: "dbo",
                table: "SeriesCourses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesPreviews_SeriesId",
                schema: "dbo",
                table: "SeriesPreviews",
                column: "SeriesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeriesPrices_DurationId",
                schema: "dbo",
                table: "SeriesPrices",
                column: "DurationId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesPrices_SeriesId_DurationId",
                schema: "dbo",
                table: "SeriesPrices",
                columns: new[] { "SeriesId", "DurationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeriesAuditLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SeriesCourses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SeriesPreviews",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SeriesPrices",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Series",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "ForSeriesOnly",
                schema: "dbo",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Courses",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldNullable: true);
        }
    }
}
