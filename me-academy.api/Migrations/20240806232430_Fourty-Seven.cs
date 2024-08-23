using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace me_academy.api.Migrations
{
    /// <inheritdoc />
    public partial class FourtySeven : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseDocuments_Courses_CourseId",
                schema: "dbo",
                table: "CourseDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseLinks_Courses_CourseId",
                schema: "dbo",
                table: "CourseLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePrices_Courses_CourseId",
                schema: "dbo",
                table: "CoursePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseQuestionResponses_Courses_CourseId",
                schema: "dbo",
                table: "CourseQuestionResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseQuestions_Courses_CourseId",
                schema: "dbo",
                table: "CourseQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseVideos_Courses_CourseId",
                schema: "dbo",
                table: "CourseVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseViewCounts_Courses_CourseId",
                schema: "dbo",
                table: "CourseViewCounts");

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
                name: "FK_SeriesCourses_Courses_CourseId",
                schema: "dbo",
                table: "SeriesCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesCourses_Series_SeriesId",
                schema: "dbo",
                table: "SeriesCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesPreviews_Series_SeriesId",
                schema: "dbo",
                table: "SeriesPreviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesPrices_Series_SeriesId",
                schema: "dbo",
                table: "SeriesPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesProgress_Courses_CourseId",
                schema: "dbo",
                table: "SeriesProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesQuestions_Courses_CourseId",
                schema: "dbo",
                table: "SeriesQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesQuestions_Series_SeriesId",
                schema: "dbo",
                table: "SeriesQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_SmeHubs_Documents_DocumentId",
                schema: "dbo",
                table: "SmeHubs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmeHubs_SmeHubTypes_TypeId",
                schema: "dbo",
                table: "SmeHubs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmeHubs_Users_CreatedById",
                schema: "dbo",
                table: "SmeHubs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmeHubs_Users_DeletedById",
                schema: "dbo",
                table: "SmeHubs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmeHubs_Users_UpdatedById",
                schema: "dbo",
                table: "SmeHubs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_Courses_CourseId",
                schema: "dbo",
                table: "UserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSeries_Series_SeriesId",
                schema: "dbo",
                table: "UserSeries");

            migrationBuilder.DropTable(
                name: "AnnotatedAgreements",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CourseAuditLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SeriesAuditLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserContents",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Courses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Series",
                schema: "dbo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SmeHubs",
                schema: "dbo",
                table: "SmeHubs");

            migrationBuilder.DropColumn(
                name: "IsExpired",
                schema: "dbo",
                table: "UserSeries");

            migrationBuilder.DropColumn(
                name: "IsExpired",
                schema: "dbo",
                table: "UserCourses");

            migrationBuilder.RenameTable(
                name: "SmeHubs",
                schema: "dbo",
                newName: "ContentBase",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_SmeHubs_UpdatedById",
                schema: "dbo",
                table: "ContentBase",
                newName: "IX_ContentBase_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_SmeHubs_TypeId",
                schema: "dbo",
                table: "ContentBase",
                newName: "IX_ContentBase_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SmeHubs_DocumentId",
                schema: "dbo",
                table: "ContentBase",
                newName: "IX_ContentBase_DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_SmeHubs_DeletedById",
                schema: "dbo",
                table: "ContentBase",
                newName: "IX_ContentBase_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_SmeHubs_CreatedById",
                schema: "dbo",
                table: "ContentBase",
                newName: "IX_ContentBase_CreatedById");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresOnUtc",
                schema: "dbo",
                table: "UserSeries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Duration",
                schema: "dbo",
                table: "UserCourses",
                type: "decimal(20,12)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresOnUtc",
                schema: "dbo",
                table: "UserCourses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ContentId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                schema: "dbo",
                table: "ContentBase",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "dbo",
                table: "ContentBase",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                schema: "dbo",
                table: "ContentBase",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                schema: "dbo",
                table: "ContentBase",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ForSeriesOnly",
                schema: "dbo",
                table: "ContentBase",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                schema: "dbo",
                table: "ContentBase",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublishedById",
                schema: "dbo",
                table: "ContentBase",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedOnUtc",
                schema: "dbo",
                table: "ContentBase",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Series_IsPublished",
                schema: "dbo",
                table: "ContentBase",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Series_PublishedById",
                schema: "dbo",
                table: "ContentBase",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Series_PublishedOnUtc",
                schema: "dbo",
                table: "ContentBase",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SmeHub_DocumentId",
                schema: "dbo",
                table: "ContentBase",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SmeHub_Price",
                schema: "dbo",
                table: "ContentBase",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentBase",
                schema: "dbo",
                table: "ContentBase",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ContentLogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentLogs", x => x.Id);
                    table.CheckConstraint("CK_ContentLog_ItemType", "[ContentType] IN ('Course', 'Series', 'SmeHub', 'AnnotatedAgreement')");
                    table.ForeignKey(
                        name: "FK_ContentLogs_ContentBase_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "dbo",
                        principalTable: "ContentBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentLogs_ContentBase_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "dbo",
                        principalTable: "ContentBase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContentLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ContentId",
                schema: "dbo",
                table: "Orders",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                schema: "dbo",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentBase_Series_PublishedById",
                schema: "dbo",
                table: "ContentBase",
                column: "Series_PublishedById");

            migrationBuilder.CreateIndex(
                name: "IX_ContentBase_SmeHub_DocumentId",
                schema: "dbo",
                table: "ContentBase",
                column: "SmeHub_DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentBase_Uid",
                schema: "dbo",
                table: "ContentBase",
                column: "Uid");

            migrationBuilder.CreateIndex(
                name: "IX_ContentLogs_ContentId",
                schema: "dbo",
                table: "ContentLogs",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentLogs_CourseId",
                schema: "dbo",
                table: "ContentLogs",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentLogs_CreatedById",
                schema: "dbo",
                table: "ContentLogs",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentBase_Documents_DocumentId",
                schema: "dbo",
                table: "ContentBase",
                column: "DocumentId",
                principalSchema: "dbo",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentBase_Documents_SmeHub_DocumentId",
                schema: "dbo",
                table: "ContentBase",
                column: "SmeHub_DocumentId",
                principalSchema: "dbo",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentBase_SmeHubTypes_TypeId",
                schema: "dbo",
                table: "ContentBase",
                column: "TypeId",
                principalSchema: "dbo",
                principalTable: "SmeHubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentBase_Users_CreatedById",
                schema: "dbo",
                table: "ContentBase",
                column: "CreatedById",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentBase_Users_DeletedById",
                schema: "dbo",
                table: "ContentBase",
                column: "DeletedById",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentBase_Users_Series_PublishedById",
                schema: "dbo",
                table: "ContentBase",
                column: "Series_PublishedById",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentBase_Users_UpdatedById",
                schema: "dbo",
                table: "ContentBase",
                column: "UpdatedById",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseDocuments_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseDocuments",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLinks_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseLinks",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePrices_ContentBase_CourseId",
                schema: "dbo",
                table: "CoursePrices",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseQuestionResponses_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseQuestionResponses",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseQuestions_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseQuestions",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseVideos_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseVideos",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseViewCounts_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseViewCounts",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ContentBase_AnnotatedAgreementId",
                schema: "dbo",
                table: "Orders",
                column: "AnnotatedAgreementId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ContentBase_ContentId",
                schema: "dbo",
                table: "Orders",
                column: "ContentId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ContentBase_CourseId",
                schema: "dbo",
                table: "Orders",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ContentBase_SeriesId",
                schema: "dbo",
                table: "Orders",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ContentBase_SmeHubId",
                schema: "dbo",
                table: "Orders",
                column: "SmeHubId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                schema: "dbo",
                table: "Orders",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesCourses_ContentBase_CourseId",
                schema: "dbo",
                table: "SeriesCourses",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesCourses_ContentBase_SeriesId",
                schema: "dbo",
                table: "SeriesCourses",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesPreviews_ContentBase_SeriesId",
                schema: "dbo",
                table: "SeriesPreviews",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesPrices_ContentBase_SeriesId",
                schema: "dbo",
                table: "SeriesPrices",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesProgress_ContentBase_CourseId",
                schema: "dbo",
                table: "SeriesProgress",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesQuestions_ContentBase_CourseId",
                schema: "dbo",
                table: "SeriesQuestions",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesQuestions_ContentBase_SeriesId",
                schema: "dbo",
                table: "SeriesQuestions",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_ContentBase_CourseId",
                schema: "dbo",
                table: "UserCourses",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeries_ContentBase_SeriesId",
                schema: "dbo",
                table: "UserSeries",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "ContentBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentBase_Documents_DocumentId",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentBase_Documents_SmeHub_DocumentId",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentBase_SmeHubTypes_TypeId",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentBase_Users_CreatedById",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentBase_Users_DeletedById",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentBase_Users_Series_PublishedById",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentBase_Users_UpdatedById",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseDocuments_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseLinks_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePrices_ContentBase_CourseId",
                schema: "dbo",
                table: "CoursePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseQuestionResponses_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseQuestionResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseQuestions_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseVideos_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseViewCounts_ContentBase_CourseId",
                schema: "dbo",
                table: "CourseViewCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ContentBase_AnnotatedAgreementId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ContentBase_ContentId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ContentBase_CourseId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ContentBase_SeriesId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ContentBase_SmeHubId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesCourses_ContentBase_CourseId",
                schema: "dbo",
                table: "SeriesCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesCourses_ContentBase_SeriesId",
                schema: "dbo",
                table: "SeriesCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesPreviews_ContentBase_SeriesId",
                schema: "dbo",
                table: "SeriesPreviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesPrices_ContentBase_SeriesId",
                schema: "dbo",
                table: "SeriesPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesProgress_ContentBase_CourseId",
                schema: "dbo",
                table: "SeriesProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesQuestions_ContentBase_CourseId",
                schema: "dbo",
                table: "SeriesQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesQuestions_ContentBase_SeriesId",
                schema: "dbo",
                table: "SeriesQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_ContentBase_CourseId",
                schema: "dbo",
                table: "UserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSeries_ContentBase_SeriesId",
                schema: "dbo",
                table: "UserSeries");

            migrationBuilder.DropTable(
                name: "ContentLogs",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ContentId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentBase",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropIndex(
                name: "IX_ContentBase_Series_PublishedById",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropIndex(
                name: "IX_ContentBase_SmeHub_DocumentId",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropIndex(
                name: "IX_ContentBase_Uid",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropColumn(
                name: "ExpiresOnUtc",
                schema: "dbo",
                table: "UserSeries");

            migrationBuilder.DropColumn(
                name: "Duration",
                schema: "dbo",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "ExpiresOnUtc",
                schema: "dbo",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "ContentId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ContentType",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropColumn(
                name: "ForSeriesOnly",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropColumn(
                name: "PublishedById",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropColumn(
                name: "PublishedOnUtc",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropColumn(
                name: "Series_IsPublished",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropColumn(
                name: "Series_PublishedById",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropColumn(
                name: "Series_PublishedOnUtc",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropColumn(
                name: "SmeHub_DocumentId",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.DropColumn(
                name: "SmeHub_Price",
                schema: "dbo",
                table: "ContentBase");

            migrationBuilder.RenameTable(
                name: "ContentBase",
                schema: "dbo",
                newName: "SmeHubs",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_ContentBase_UpdatedById",
                schema: "dbo",
                table: "SmeHubs",
                newName: "IX_SmeHubs_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ContentBase_TypeId",
                schema: "dbo",
                table: "SmeHubs",
                newName: "IX_SmeHubs_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentBase_DocumentId",
                schema: "dbo",
                table: "SmeHubs",
                newName: "IX_SmeHubs_DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentBase_DeletedById",
                schema: "dbo",
                table: "SmeHubs",
                newName: "IX_SmeHubs_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_ContentBase_CreatedById",
                schema: "dbo",
                table: "SmeHubs",
                newName: "IX_SmeHubs_CreatedById");

            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                schema: "dbo",
                table: "UserSeries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                schema: "dbo",
                table: "UserCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                schema: "dbo",
                table: "SmeHubs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "dbo",
                table: "SmeHubs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                schema: "dbo",
                table: "SmeHubs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmeHubs",
                schema: "dbo",
                table: "SmeHubs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AnnotatedAgreements",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Uid = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    ForSeriesOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedById = table.Column<int>(type: "int", nullable: true),
                    PublishedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Uid = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Courses_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Series",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    PublishedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Uid = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Series_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Series_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Series_Users_PublishedById",
                        column: x => x.PublishedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Series_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserContents",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContents_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "dbo",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContents_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseAuditLogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseAuditLogs_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "dbo",
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseAuditLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeriesAuditLogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesAuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeriesAuditLogs_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "dbo",
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesAuditLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CourseAuditLogs_CourseId",
                schema: "dbo",
                table: "CourseAuditLogs",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAuditLogs_CreatedById",
                schema: "dbo",
                table: "CourseAuditLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CreatedById",
                schema: "dbo",
                table: "Courses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DeletedById",
                schema: "dbo",
                table: "Courses",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Uid",
                schema: "dbo",
                table: "Courses",
                column: "Uid");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_UpdatedById",
                schema: "dbo",
                table: "Courses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Series_CreatedById",
                schema: "dbo",
                table: "Series",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Series_DeletedById",
                schema: "dbo",
                table: "Series",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Series_PublishedById",
                schema: "dbo",
                table: "Series",
                column: "PublishedById");

            migrationBuilder.CreateIndex(
                name: "IX_Series_UpdatedById",
                schema: "dbo",
                table: "Series",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesAuditLogs_CreatedById",
                schema: "dbo",
                table: "SeriesAuditLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesAuditLogs_SeriesId",
                schema: "dbo",
                table: "SeriesAuditLogs",
                column: "SeriesId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CourseDocuments_Courses_CourseId",
                schema: "dbo",
                table: "CourseDocuments",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLinks_Courses_CourseId",
                schema: "dbo",
                table: "CourseLinks",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePrices_Courses_CourseId",
                schema: "dbo",
                table: "CoursePrices",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseQuestionResponses_Courses_CourseId",
                schema: "dbo",
                table: "CourseQuestionResponses",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseQuestions_Courses_CourseId",
                schema: "dbo",
                table: "CourseQuestions",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseVideos_Courses_CourseId",
                schema: "dbo",
                table: "CourseVideos",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseViewCounts_Courses_CourseId",
                schema: "dbo",
                table: "CourseViewCounts",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_SeriesCourses_Courses_CourseId",
                schema: "dbo",
                table: "SeriesCourses",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesCourses_Series_SeriesId",
                schema: "dbo",
                table: "SeriesCourses",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "Series",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesPreviews_Series_SeriesId",
                schema: "dbo",
                table: "SeriesPreviews",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesPrices_Series_SeriesId",
                schema: "dbo",
                table: "SeriesPrices",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesProgress_Courses_CourseId",
                schema: "dbo",
                table: "SeriesProgress",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesQuestions_Courses_CourseId",
                schema: "dbo",
                table: "SeriesQuestions",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesQuestions_Series_SeriesId",
                schema: "dbo",
                table: "SeriesQuestions",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmeHubs_Documents_DocumentId",
                schema: "dbo",
                table: "SmeHubs",
                column: "DocumentId",
                principalSchema: "dbo",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmeHubs_SmeHubTypes_TypeId",
                schema: "dbo",
                table: "SmeHubs",
                column: "TypeId",
                principalSchema: "dbo",
                principalTable: "SmeHubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmeHubs_Users_CreatedById",
                schema: "dbo",
                table: "SmeHubs",
                column: "CreatedById",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmeHubs_Users_DeletedById",
                schema: "dbo",
                table: "SmeHubs",
                column: "DeletedById",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SmeHubs_Users_UpdatedById",
                schema: "dbo",
                table: "SmeHubs",
                column: "UpdatedById",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_Courses_CourseId",
                schema: "dbo",
                table: "UserCourses",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeries_Series_SeriesId",
                schema: "dbo",
                table: "UserSeries",
                column: "SeriesId",
                principalSchema: "dbo",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
