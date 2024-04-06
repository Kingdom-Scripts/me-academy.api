using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Five : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_DurationType_Name",
                schema: "dbo",
                table: "DurationTypes");

            migrationBuilder.RenameColumn(
                name: "TotalDays",
                schema: "dbo",
                table: "DurationTypes",
                newName: "Count");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "DurationTypes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "dbo",
                table: "DurationTypes",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_DurationType_Type",
                schema: "dbo",
                table: "DurationTypes",
                sql: "[Type] IN ('Days', 'Weeks', 'Months', 'Years')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_DurationType_Type",
                schema: "dbo",
                table: "DurationTypes");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "dbo",
                table: "DurationTypes");

            migrationBuilder.RenameColumn(
                name: "Count",
                schema: "dbo",
                table: "DurationTypes",
                newName: "TotalDays");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "DurationTypes",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddCheckConstraint(
                name: "CK_DurationType_Name",
                schema: "dbo",
                table: "DurationTypes",
                sql: "[Name] IN ('Days', 'Weeks', 'Months', 'Years')");
        }
    }
}
