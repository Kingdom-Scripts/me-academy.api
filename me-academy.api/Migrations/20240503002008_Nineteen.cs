using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Nineteen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourseVideos_CourseId",
                schema: "dbo",
                table: "CourseVideos");

            migrationBuilder.DropColumn(
                name: "VideoIsUploaded",
                schema: "dbo",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "PreviewThumbnailUrl",
                schema: "dbo",
                table: "CourseVideos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewVideoId",
                schema: "dbo",
                table: "CourseVideos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VideoDuration",
                schema: "dbo",
                table: "CourseVideos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserCourses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Progress = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchasedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseVideos_CourseId",
                schema: "dbo",
                table: "CourseVideos",
                column: "CourseId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCourses",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_CourseVideos_CourseId",
                schema: "dbo",
                table: "CourseVideos");

            migrationBuilder.DropColumn(
                name: "PreviewThumbnailUrl",
                schema: "dbo",
                table: "CourseVideos");

            migrationBuilder.DropColumn(
                name: "PreviewVideoId",
                schema: "dbo",
                table: "CourseVideos");

            migrationBuilder.DropColumn(
                name: "VideoDuration",
                schema: "dbo",
                table: "CourseVideos");

            migrationBuilder.AddColumn<bool>(
                name: "VideoIsUploaded",
                schema: "dbo",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CourseVideos_CourseId",
                schema: "dbo",
                table: "CourseVideos",
                column: "CourseId");
        }
    }
}
