﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfcDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddClocksToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Clocks");

            migrationBuilder.AddColumn<long>(
                name: "TimeOffset",
                table: "Clocks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Todos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Clocks",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "TimeOffset",
                value: 0L);

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Deadline", "Description", "Name", "Status", "UserId" },
                values: new object[] { new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"), new DateTime(2024, 5, 17, 9, 10, 14, 364, DateTimeKind.Utc).AddTicks(4747), "hello description", "Hello", 1, new Guid("5f3bb5af-e982-4a8b-8590-b620597a7360") });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_UserId",
                table: "Todos",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");

            migrationBuilder.DropColumn(
                name: "TimeOffset",
                table: "Clocks");

            migrationBuilder.AddColumn<char>(
                name: "TimeZone",
                table: "Clocks",
                type: "character(1)",
                nullable: false,
                defaultValue: ' ');

            migrationBuilder.UpdateData(
                table: "Clocks",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "TimeZone",
                value: 'G');
        }
    }
}
