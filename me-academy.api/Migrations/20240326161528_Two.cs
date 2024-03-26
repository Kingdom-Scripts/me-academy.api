using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Two : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                schema: "dbo",
                table: "Codes",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                schema: "dbo",
                table: "Codes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Codes_OwnerId",
                schema: "dbo",
                table: "Codes",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Codes_Users_OwnerId",
                schema: "dbo",
                table: "Codes",
                column: "OwnerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Codes_Users_OwnerId",
                schema: "dbo",
                table: "Codes");

            migrationBuilder.DropIndex(
                name: "IX_Codes_OwnerId",
                schema: "dbo",
                table: "Codes");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "dbo",
                table: "Codes");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                schema: "dbo",
                table: "Codes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);
        }
    }
}
