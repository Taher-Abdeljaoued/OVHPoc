using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OVHPoc.Shared.Migrations
{
    public partial class AddForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Mailbox_MailboxId",
                table: "Folders",
                column: "MailboxId",
                principalTable: "Mailbox",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Mailbox_MailboxId",
                table: "Folders");
        }
    }
}
