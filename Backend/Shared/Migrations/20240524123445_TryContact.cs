using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class TryContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTag_Events_EventsId",
                table: "EventTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.RenameIndex(
                name: "IX_Events_UserId",
                table: "Event",
                newName: "IX_Event_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    User1Id = table.Column<Guid>(type: "uuid", nullable: false),
                    User2Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.id);
                    table.ForeignKey(
                        name: "FK_Contact_Users_User1Id",
                        column: x => x.User1Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contact_Users_User2Id",
                        column: x => x.User2Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 31, 12, 34, 45, 672, DateTimeKind.Utc).AddTicks(712));

            migrationBuilder.CreateIndex(
                name: "IX_Contact_User1Id",
                table: "Contact",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_User2Id",
                table: "Contact",
                column: "User2Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Users_UserId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTag_Event_EventsId",
                table: "EventTag");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.RenameIndex(
                name: "IX_Event_UserId",
                table: "Events",
                newName: "IX_Events_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 31, 11, 47, 22, 823, DateTimeKind.Utc).AddTicks(1575));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5f3bb5af-e982-4a8b-8590-b620597a7360"),
                column: "UserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
