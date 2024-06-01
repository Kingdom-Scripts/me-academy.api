using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class ThirtyNine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmeHubs_SmeHubTypess_TypeId",
                schema: "dbo",
                table: "SmeHubs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SmeHubTypess",
                schema: "dbo",
                table: "SmeHubTypess");

            migrationBuilder.RenameTable(
                name: "SmeHubTypess",
                schema: "dbo",
                newName: "SmeHubTypes",
                newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmeHubTypes",
                schema: "dbo",
                table: "SmeHubTypes",
                column: "Id");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_SmeHubTypes",
                schema: "dbo",
                table: "SmeHubTypes");

            migrationBuilder.RenameTable(
                name: "SmeHubTypes",
                schema: "dbo",
                newName: "SmeHubTypess",
                newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmeHubTypess",
                schema: "dbo",
                table: "SmeHubTypess",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SmeHubs_SmeHubTypess_TypeId",
                schema: "dbo",
                table: "SmeHubs",
                column: "TypeId",
                principalSchema: "dbo",
                principalTable: "SmeHubTypess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
