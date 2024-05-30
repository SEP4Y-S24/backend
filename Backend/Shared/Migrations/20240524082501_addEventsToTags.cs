using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class addEventsToTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Events_EventId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_EventId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "EventTag",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTag", x => new { x.CategoriesId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_EventTag_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventTag_Tags_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 31, 8, 25, 1, 798, DateTimeKind.Utc).AddTicks(3257));

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_EventsId",
                table: "EventTag",
                column: "EventsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTag");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Tags",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 31, 7, 51, 16, 968, DateTimeKind.Utc).AddTicks(3543));

            migrationBuilder.CreateIndex(
                name: "IX_Tags_EventId",
                table: "Tags",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Events_EventId",
                table: "Tags",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
