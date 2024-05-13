using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfcDatabase.Migrations
{
    /// <inheritdoc />
    public partial class initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alarms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClockId = table.Column<Guid>(type: "uuid", nullable: false),
                    SetOffTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsSnoozed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alarms_Clocks_ClockId",
                        column: x => x.ClockId,
                        principalTable: "Clocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Alarms",
                columns: new[] { "Id", "ClockId", "IsActive", "IsSnoozed", "SetOffTime" },
                values: new object[] { new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"), new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"), true, false, new DateTime(2024, 5, 13, 10, 1, 39, 498, DateTimeKind.Utc).AddTicks(7483) });

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 20, 9, 1, 39, 498, DateTimeKind.Utc).AddTicks(7457));

            migrationBuilder.CreateIndex(
                name: "IX_Alarms_ClockId",
                table: "Alarms",
                column: "ClockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alarms");

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 17, 9, 10, 14, 364, DateTimeKind.Utc).AddTicks(4747));
        }
    }
}
