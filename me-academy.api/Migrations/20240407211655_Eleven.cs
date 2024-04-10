using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Eleven : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseLink_Courses_CourseId",
                schema: "dbo",
                table: "CourseLink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseLink",
                schema: "dbo",
                table: "CourseLink");

            migrationBuilder.RenameTable(
                name: "CourseLink",
                schema: "dbo",
                newName: "CourseLinks",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_CourseLink_CourseId",
                schema: "dbo",
                table: "CourseLinks",
                newName: "IX_CourseLinks_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseLinks",
                schema: "dbo",
                table: "CourseLinks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLinks_Courses_CourseId",
                schema: "dbo",
                table: "CourseLinks",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseLinks_Courses_CourseId",
                schema: "dbo",
                table: "CourseLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseLinks",
                schema: "dbo",
                table: "CourseLinks");

            migrationBuilder.RenameTable(
                name: "CourseLinks",
                schema: "dbo",
                newName: "CourseLink",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_CourseLinks_CourseId",
                schema: "dbo",
                table: "CourseLink",
                newName: "IX_CourseLink_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseLink",
                schema: "dbo",
                table: "CourseLink",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLink_Courses_CourseId",
                schema: "dbo",
                table: "CourseLink",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
