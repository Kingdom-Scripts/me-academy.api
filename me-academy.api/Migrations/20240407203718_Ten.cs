using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Ten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "dbo",
                table: "Courses",
                newName: "DeletedOnUtc");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "dbo",
                table: "CoursePrices",
                newName: "DeletedOnUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedOnUtc",
                schema: "dbo",
                table: "Courses",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "DeletedOnUtc",
                schema: "dbo",
                table: "CoursePrices",
                newName: "DeletedOn");
        }
    }
}
