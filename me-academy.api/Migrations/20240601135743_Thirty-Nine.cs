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
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SmeHubs_SmeHubTypess_TypeId",
            //    schema: "dbo",
            //    table: "SmeHubs");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_SmeHubTypess",
            //    schema: "dbo",
            //    table: "SmeHubTypess");

            //migrationBuilder.RenameTable(
            //    name: "SmeHubTypess",
            //    schema: "dbo",
            //    newName: "SmeHubTypes",
            //    newSchema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "dbo",
                table: "Discounts",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_SmeHubTypes",
            //    schema: "dbo",
            //    table: "SmeHubTypes",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SmeHubs_SmeHubTypes_TypeId",
            //    schema: "dbo",
            //    table: "SmeHubs",
            //    column: "TypeId",
            //    principalSchema: "dbo",
            //    principalTable: "SmeHubTypes",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SmeHubs_SmeHubTypes_TypeId",
            //    schema: "dbo",
            //    table: "SmeHubs");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_SmeHubTypes",
            //    schema: "dbo",
            //    table: "SmeHubTypes");

            //migrationBuilder.RenameTable(
            //    name: "SmeHubTypes",
            //    schema: "dbo",
            //    newName: "SmeHubTypess",
            //    newSchema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "dbo",
                table: "Discounts",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_SmeHubTypess",
            //    schema: "dbo",
            //    table: "SmeHubTypess",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SmeHubs_SmeHubTypess_TypeId",
            //    schema: "dbo",
            //    table: "SmeHubs",
            //    column: "TypeId",
            //    principalSchema: "dbo",
            //    principalTable: "SmeHubTypess",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
