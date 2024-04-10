using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Twelve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseDocuments",
                schema: "dbo",
                table: "CourseDocuments");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "CourseDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                schema: "dbo",
                table: "CourseDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                schema: "dbo",
                table: "CourseDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseDocuments",
                schema: "dbo",
                table: "CourseDocuments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDocuments_CourseId_DocumentId",
                schema: "dbo",
                table: "CourseDocuments",
                columns: new[] { "CourseId", "DocumentId" });

            migrationBuilder.CreateIndex(
                name: "IX_CourseDocuments_CreatedById",
                schema: "dbo",
                table: "CourseDocuments",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseDocuments_Users_CreatedById",
                schema: "dbo",
                table: "CourseDocuments",
                column: "CreatedById",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseDocuments_Users_CreatedById",
                schema: "dbo",
                table: "CourseDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseDocuments",
                schema: "dbo",
                table: "CourseDocuments");

            migrationBuilder.DropIndex(
                name: "IX_CourseDocuments_CourseId_DocumentId",
                schema: "dbo",
                table: "CourseDocuments");

            migrationBuilder.DropIndex(
                name: "IX_CourseDocuments_CreatedById",
                schema: "dbo",
                table: "CourseDocuments");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "CourseDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                schema: "dbo",
                table: "CourseDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "dbo",
                table: "CourseDocuments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseDocuments",
                schema: "dbo",
                table: "CourseDocuments",
                columns: new[] { "CourseId", "DocumentId" });
        }
    }
}
