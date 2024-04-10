using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Seven : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePrice_Courses_CourseId",
                schema: "dbo",
                table: "CoursePrice");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePrice_DurationTypes_DurationTypeId",
                schema: "dbo",
                table: "CoursePrice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoursePrice",
                schema: "dbo",
                table: "CoursePrice");

            migrationBuilder.RenameTable(
                name: "CoursePrice",
                schema: "dbo",
                newName: "CoursePrices",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_CoursePrice_DurationTypeId",
                schema: "dbo",
                table: "CoursePrices",
                newName: "IX_CoursePrices_DurationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CoursePrice_CourseId",
                schema: "dbo",
                table: "CoursePrices",
                newName: "IX_CoursePrices_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoursePrices",
                schema: "dbo",
                table: "CoursePrices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePrices_Courses_CourseId",
                schema: "dbo",
                table: "CoursePrices",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePrices_DurationTypes_DurationTypeId",
                schema: "dbo",
                table: "CoursePrices",
                column: "DurationTypeId",
                principalSchema: "dbo",
                principalTable: "DurationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePrices_Courses_CourseId",
                schema: "dbo",
                table: "CoursePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePrices_DurationTypes_DurationTypeId",
                schema: "dbo",
                table: "CoursePrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoursePrices",
                schema: "dbo",
                table: "CoursePrices");

            migrationBuilder.RenameTable(
                name: "CoursePrices",
                schema: "dbo",
                newName: "CoursePrice",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_CoursePrices_DurationTypeId",
                schema: "dbo",
                table: "CoursePrice",
                newName: "IX_CoursePrice_DurationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CoursePrices_CourseId",
                schema: "dbo",
                table: "CoursePrice",
                newName: "IX_CoursePrice_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoursePrice",
                schema: "dbo",
                table: "CoursePrice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePrice_Courses_CourseId",
                schema: "dbo",
                table: "CoursePrice",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePrice_DurationTypes_DurationTypeId",
                schema: "dbo",
                table: "CoursePrice",
                column: "DurationTypeId",
                principalSchema: "dbo",
                principalTable: "DurationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
