using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class TryContact2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Users_User1Id",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Users_User2Id",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Users_UserId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTag_Event_EventsId",
                table: "EventTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contact",
                table: "Contact");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.RenameTable(
                name: "Contact",
                newName: "Contacts");

            migrationBuilder.RenameIndex(
                name: "IX_Event_UserId",
                table: "Events",
                newName: "IX_Events_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_User2Id",
                table: "Contacts",
                newName: "IX_Contacts_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_User1Id",
                table: "Contacts",
                newName: "IX_Contacts_User1Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts",
                column: "id");

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 31, 12, 49, 16, 415, DateTimeKind.Utc).AddTicks(9599));

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Users_User1Id",
                table: "Contacts",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Users_User2Id",
                table: "Contacts",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTag_Events_EventsId",
                table: "EventTag",
                column: "EventsId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_User1Id",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_User2Id",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTag_Events_EventsId",
                table: "EventTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.RenameTable(
                name: "Contacts",
                newName: "Contact");

            migrationBuilder.RenameIndex(
                name: "IX_Events_UserId",
                table: "Event",
                newName: "IX_Event_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_User2Id",
                table: "Contact",
                newName: "IX_Contact_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_User1Id",
                table: "Contact",
                newName: "IX_Contact_User1Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contact",
                table: "Contact",
                column: "id");

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 31, 12, 34, 45, 672, DateTimeKind.Utc).AddTicks(712));

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Users_User1Id",
                table: "Contact",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Users_User2Id",
                table: "Contact",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Users_UserId",
                table: "Event",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTag_Event_EventsId",
                table: "EventTag",
                column: "EventsId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
