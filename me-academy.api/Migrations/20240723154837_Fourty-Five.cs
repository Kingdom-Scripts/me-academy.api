using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class FourtyFive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Domain",
                schema: "dbo",
                table: "Logins",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logins_UserId_Domain",
                schema: "dbo",
                table: "Logins",
                columns: new[] { "UserId", "Domain" },
                unique: true,
                filter: "[Domain] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logins_UserId_Domain",
                schema: "dbo",
                table: "Logins");

            migrationBuilder.AlterColumn<string>(
                name: "Domain",
                schema: "dbo",
                table: "Logins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
