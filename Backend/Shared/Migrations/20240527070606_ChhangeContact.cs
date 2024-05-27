using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class ChhangeContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_User1Id",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_User2Id",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "User2Id",
                table: "Contacts",
                newName: "User2id");

            migrationBuilder.RenameColumn(
                name: "User1Id",
                table: "Contacts",
                newName: "User1id");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_User2Id",
                table: "Contacts",
                newName: "IX_Contacts_User2id");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_User1Id",
                table: "Contacts",
                newName: "IX_Contacts_User1id");

            migrationBuilder.AddColumn<string>(
                name: "Email1",
                table: "Contacts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email2",
                table: "Contacts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 6, 3, 7, 6, 5, 900, DateTimeKind.Utc).AddTicks(8967));

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Users_User1id",
                table: "Contacts",
                column: "User1id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Users_User2id",
                table: "Contacts",
                column: "User2id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_User1id",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_User2id",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Email1",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Email2",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "User2id",
                table: "Contacts",
                newName: "User2Id");

            migrationBuilder.RenameColumn(
                name: "User1id",
                table: "Contacts",
                newName: "User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_User2id",
                table: "Contacts",
                newName: "IX_Contacts_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_User1id",
                table: "Contacts",
                newName: "IX_Contacts_User1Id");

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
        }
    }
}
