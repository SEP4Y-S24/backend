using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAlarm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "SetOffTime",
                table: "Alarms",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.UpdateData(
                table: "Alarms",
                keyColumn: "Id",
                keyValue: new Guid("ac96066e-c7da-4b53-9203-d1bf4b5a88b9"),
                column: "SetOffTime",
                value: new TimeOnly(1, 20, 0));

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 29, 11, 17, 29, 283, DateTimeKind.Utc).AddTicks(4940));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SetOffTime",
                table: "Alarms",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.UpdateData(
                table: "Alarms",
                keyColumn: "Id",
                keyValue: new Guid("ac96066e-c7da-4b53-9203-d1bf4b5a88b9"),
                column: "SetOffTime",
                value: new DateTime(2024, 5, 21, 12, 32, 31, 836, DateTimeKind.Utc).AddTicks(4785));

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                column: "Deadline",
                value: new DateTime(2024, 5, 28, 11, 32, 31, 836, DateTimeKind.Utc).AddTicks(4802));
        }
    }
}
