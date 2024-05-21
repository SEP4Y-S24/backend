﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAlarmService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alarms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ClockId = table.Column<Guid>(type: "uuid", nullable: false),
                    SetOffTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsSnoozed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarms", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "Alarms",
                columns: new[] { "Id", "ClockId", "IsActive", "IsSnoozed", "Name", "SetOffTime" },
                values: new object[] { new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"), new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"), true, false, "Default alarm", new DateTime(2024, 5, 21, 8, 44, 53, 627, DateTimeKind.Utc).AddTicks(3142) });

            migrationBuilder.InsertData(
                table: "Users",
                column: "Id",
                value: new Guid("5f3bb5af-e982-4a8b-8590-b620597a7360"));

            migrationBuilder.InsertData(
                table: "Clocks",
                columns: new[] { "Id", "Name", "OwnerId", "TimeOffset" },
                values: new object[] { new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"), "Test Clock", new Guid("5f3bb5af-e982-4a8b-8590-b620597a7360"), 0L });

            migrationBuilder.CreateIndex(
                name: "IX_Clocks_OwnerId",
                table: "Clocks",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alarms");

            migrationBuilder.DropTable(
                name: "Clocks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}