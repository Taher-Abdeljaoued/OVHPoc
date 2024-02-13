using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OVHPoc.Shared.Migrations
{
    public partial class AddDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateUtc",
                table: "MailMigrationRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateUtc",
                table: "MailMigrationRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mails_FolderId",
                table: "Mails",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mails_Folders_FolderId",
                table: "Mails",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mails_Folders_FolderId",
                table: "Mails");

            migrationBuilder.DropIndex(
                name: "IX_Mails_FolderId",
                table: "Mails");

            migrationBuilder.DropColumn(
                name: "CreationDateUtc",
                table: "MailMigrationRequests");

            migrationBuilder.DropColumn(
                name: "UpdateDateUtc",
                table: "MailMigrationRequests");
        }
    }
}
