using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class TwentyEight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "TypeId",
            //    schema: "dbo",
            //    table: "SmeHubs",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AnnotatedAgreements",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uid = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnotatedAgreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnotatedAgreements_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "dbo",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnotatedAgreements_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnotatedAgreements_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnnotatedAgreements_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            //migrationBuilder.CreateTable(
            //    name: "SmeHubTypes",
            //    schema: "dbo",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SmeHubTypes", x => x.Id);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_SmeHubs_TypeId",
            //    schema: "dbo",
            //    table: "SmeHubs",
            //    column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnotatedAgreements_CreatedById",
                schema: "dbo",
                table: "AnnotatedAgreements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AnnotatedAgreements_DeletedById",
                schema: "dbo",
                table: "AnnotatedAgreements",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_AnnotatedAgreements_DocumentId",
                schema: "dbo",
                table: "AnnotatedAgreements",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnotatedAgreements_UpdatedById",
                schema: "dbo",
                table: "AnnotatedAgreements",
                column: "UpdatedById");

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_SmeHubs_SmeHubTypes_TypeId",
        //        schema: "dbo",
        //        table: "SmeHubs",
        //        column: "TypeId",
        //        principalSchema: "dbo",
        //        principalTable: "SmeHubTypes",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SmeHubs_SmeHubTypes_TypeId",
            //    schema: "dbo",
            //    table: "SmeHubs");

            migrationBuilder.DropTable(
                name: "AnnotatedAgreements",
                schema: "dbo");

            //migrationBuilder.DropTable(
            //    name: "SmeHubTypes",
            //    schema: "dbo");

            //migrationBuilder.DropIndex(
            //    name: "IX_SmeHubs_TypeId",
            //    schema: "dbo",
            //    table: "SmeHubs");

            //migrationBuilder.DropColumn(
            //    name: "TypeId",
            //    schema: "dbo",
            //    table: "SmeHubs");
        }
    }
}
