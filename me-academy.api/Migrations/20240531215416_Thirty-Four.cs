using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class ThirtyFour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                schema: "dbo",
                table: "UserContents",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "UserContentId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserContents_OrderId",
                schema: "dbo",
                table: "UserContents",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserContents_UserId",
                schema: "dbo",
                table: "UserContents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AnnotatedAgreementId",
                schema: "dbo",
                table: "Orders",
                column: "AnnotatedAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CourseId",
                schema: "dbo",
                table: "Orders",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SeriesId",
                schema: "dbo",
                table: "Orders",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SmeHubId",
                schema: "dbo",
                table: "Orders",
                column: "SmeHubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AnnotatedAgreements_AnnotatedAgreementId",
                schema: "dbo",
                table: "Orders",
                column: "AnnotatedAgreementId",
                principalSchema: "dbo",
                principalTable: "AnnotatedAgreements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Courses_CourseId",
                schema: "dbo",
                table: "Orders",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Series_SeriesId",
                schema: "dbo",
                table: "Orders",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "Series",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_SmeHubs_SmeHubId",
                schema: "dbo",
                table: "Orders",
                column: "SmeHubId",
                principalSchema: "dbo",
                principalTable: "SmeHubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContents_Orders_OrderId",
                schema: "dbo",
                table: "UserContents",
                column: "OrderId",
                principalSchema: "dbo",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContents_Users_UserId",
                schema: "dbo",
                table: "UserContents",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AnnotatedAgreements_AnnotatedAgreementId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Courses_CourseId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Series_SeriesId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_SmeHubs_SmeHubId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContents_Orders_OrderId",
                schema: "dbo",
                table: "UserContents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContents_Users_UserId",
                schema: "dbo",
                table: "UserContents");

            migrationBuilder.DropIndex(
                name: "IX_UserContents_OrderId",
                schema: "dbo",
                table: "UserContents");

            migrationBuilder.DropIndex(
                name: "IX_UserContents_UserId",
                schema: "dbo",
                table: "UserContents");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AnnotatedAgreementId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CourseId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SeriesId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SmeHubId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserContentId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                schema: "dbo",
                table: "UserContents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
