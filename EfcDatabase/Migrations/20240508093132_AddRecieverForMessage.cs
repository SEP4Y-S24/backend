using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfcDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddRecieverForMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Clocks_RecieverId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "RecieverId",
                table: "Messages",
                newName: "ClockId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_RecieverId",
                table: "Messages",
                newName: "IX_Messages_ClockId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Clocks_ClockId",
                table: "Messages",
                column: "ClockId",
                principalTable: "Clocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Clocks_ClockId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ReceiverId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "ClockId",
                table: "Messages",
                newName: "RecieverId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ClockId",
                table: "Messages",
                newName: "IX_Messages_RecieverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Clocks_RecieverId",
                table: "Messages",
                column: "RecieverId",
                principalTable: "Clocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
