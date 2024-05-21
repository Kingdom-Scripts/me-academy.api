using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class TwentySeven : Migration
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

            migrationBuilder.CreateTable(
                name: "SmeHubTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmeHubTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmeHubs_TypeId",
                schema: "dbo",
                table: "SmeHubs",
                column: "TypeId");

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

            migrationBuilder.DropTable(
                name: "SmeHubTypes",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_SmeHubs_TypeId",
                schema: "dbo",
                table: "SmeHubs");

            migrationBuilder.DropColumn(
                name: "TypeId",
                schema: "dbo",
                table: "SmeHubs");
        }
    }
}
