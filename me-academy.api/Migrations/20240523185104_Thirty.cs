using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class Thirty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountCode",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Available",
                schema: "dbo",
                table: "Discounts",
                newName: "TotalLeft");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                schema: "dbo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DurationId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountApplied",
                schema: "dbo",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Authorization_Url",
                schema: "dbo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Access_Code",
                schema: "dbo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "Count",
                schema: "dbo",
                table: "Durations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserContents",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DiscountId",
                schema: "dbo",
                table: "Orders",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DurationId",
                schema: "dbo",
                table: "Orders",
                column: "DurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Discounts_DiscountId",
                schema: "dbo",
                table: "Orders",
                column: "DiscountId",
                principalSchema: "dbo",
                principalTable: "Discounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Durations_DurationId",
                schema: "dbo",
                table: "Orders",
                column: "DurationId",
                principalSchema: "dbo",
                principalTable: "Durations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Discounts_DiscountId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Durations_DurationId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "UserContents",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DiscountId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DurationId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "TotalLeft",
                schema: "dbo",
                table: "Discounts",
                newName: "Available");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                schema: "dbo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DurationId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountApplied",
                schema: "dbo",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Authorization_Url",
                schema: "dbo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Access_Code",
                schema: "dbo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscountCode",
                schema: "dbo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Count",
                schema: "dbo",
                table: "Durations",
                type: "int",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");
        }
    }
}
