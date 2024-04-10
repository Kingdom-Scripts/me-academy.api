using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Eight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePrices_DurationTypes_DurationTypeId",
                schema: "dbo",
                table: "CoursePrices");

            migrationBuilder.DropTable(
                name: "DurationTypes",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "DurationTypeId",
                schema: "dbo",
                table: "CoursePrices",
                newName: "DurationId");

            migrationBuilder.RenameIndex(
                name: "IX_CoursePrices_DurationTypeId",
                schema: "dbo",
                table: "CoursePrices",
                newName: "IX_CoursePrices_DurationId");

            migrationBuilder.CreateTable(
                name: "Durations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Count = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Durations", x => x.Id);
                    table.CheckConstraint("CK_DurationType_Type", "[Type] IN ('Days', 'Weeks', 'Months', 'Years')");
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePrices_Durations_DurationId",
                schema: "dbo",
                table: "CoursePrices",
                column: "DurationId",
                principalSchema: "dbo",
                principalTable: "Durations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePrices_Durations_DurationId",
                schema: "dbo",
                table: "CoursePrices");

            migrationBuilder.DropTable(
                name: "Durations",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "DurationId",
                schema: "dbo",
                table: "CoursePrices",
                newName: "DurationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CoursePrices_DurationId",
                schema: "dbo",
                table: "CoursePrices",
                newName: "IX_CoursePrices_DurationTypeId");

            migrationBuilder.CreateTable(
                name: "DurationTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DurationTypes", x => x.Id);
                    table.CheckConstraint("CK_DurationType_Type", "[Type] IN ('Days', 'Weeks', 'Months', 'Years')");
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePrices_DurationTypes_DurationTypeId",
                schema: "dbo",
                table: "CoursePrices",
                column: "DurationTypeId",
                principalSchema: "dbo",
                principalTable: "DurationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
