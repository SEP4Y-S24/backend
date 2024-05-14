﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlarmServices.Migrations
{
    /// <inheritdoc />
    public partial class intialAlarmService3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TimeOffset = table.Column<long>(type: "bigint", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clocks_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClockId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Clocks_ClockId",
                        column: x => x.ClockId,
                        principalTable: "Clocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                column: "Id",
                value: new Guid("5f3bb5af-e982-4a8b-8590-b620597a7360"));

            migrationBuilder.InsertData(
                table: "Clocks",
                columns: new[] { "Id", "Name", "OwnerId", "TimeOffset" },
                values: new object[] { new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"), "Test Clock", new Guid("5f3bb5af-e982-4a8b-8590-b620597a7360"), 0L });

            migrationBuilder.InsertData(
                table: "Alarms",
                columns: new[] { "Id", "ClockId", "IsActive", "IsSnoozed", "SetOffTime" },
                values: new object[] { new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"), new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"), true, false, new DateTime(2024, 5, 14, 12, 27, 24, 164, DateTimeKind.Utc).AddTicks(4037) });

            migrationBuilder.CreateIndex(
                name: "IX_Alarms_ClockId",
                table: "Alarms",
                column: "ClockId");

            migrationBuilder.CreateIndex(
                name: "IX_Clocks_OwnerId",
                table: "Clocks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ClockId",
                table: "Messages",
                column: "ClockId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alarms");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Clocks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
