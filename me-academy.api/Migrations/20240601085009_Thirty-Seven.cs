using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class ThirtySeven : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Progress",
                schema: "dbo",
                table: "UserCourses",
                type: "decimal(20,12)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,10)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Progress",
                schema: "dbo",
                table: "UserCourses",
                type: "decimal(10,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,12)");
        }
    }
}
