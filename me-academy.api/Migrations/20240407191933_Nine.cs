using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Nine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CoursePrices_CourseId",
                schema: "dbo",
                table: "CoursePrices");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "dbo",
                table: "CoursePrices");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePrices_CourseId_DurationId",
                schema: "dbo",
                table: "CoursePrices",
                columns: new[] { "CourseId", "DurationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CoursePrices_CourseId_DurationId",
                schema: "dbo",
                table: "CoursePrices");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "CoursePrices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CoursePrices_CourseId",
                schema: "dbo",
                table: "CoursePrices",
                column: "CourseId");
        }
    }
}
