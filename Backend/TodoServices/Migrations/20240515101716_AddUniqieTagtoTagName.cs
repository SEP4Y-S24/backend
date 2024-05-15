using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoServices.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqieTagtoTagName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 22, 10, 17, 16, 74, DateTimeKind.Utc).AddTicks(2719));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 22, 9, 30, 27, 488, DateTimeKind.Utc).AddTicks(4217));
        }
    }
}
