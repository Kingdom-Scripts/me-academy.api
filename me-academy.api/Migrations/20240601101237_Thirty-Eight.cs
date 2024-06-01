using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class ThirtyEight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                schema: "dbo",
                table: "SmeHubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_SmeHubs_SmeHubTypes_TypeId",
                schema: "dbo",
                table: "SmeHubs",
                column: "TypeId",
                principalSchema: "dbo",
                principalTable: "SmeHubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmeHubs_SmeHubTypes_TypeId",
                schema: "dbo",
                table: "SmeHubs");

            migrationBuilder.DropColumn(
                name: "TypeId",
                schema: "dbo",
                table: "SmeHubs");
        }
    }
}
