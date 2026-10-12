using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemberShip.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Users_UserId",
                table: "Todos");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Todos",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "IsActive", "IsAdmin", "Password" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1@example.com", true, false, "AAmHUvRRDQLxYolb2TP+yOiOadcfTtc+AwajPQZ9JWpi1B7pAL4xz4dG9yg2p8Xe5w==" },
                    { 2, new DateTime(1990, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2@example.com", true, false, "ACIWSmVkigttTZdMj+EHQj9oEKONLLA7MrYITne3uwpHwVljbe4pBfAs0nXcLSgyWg==" },
                    { 3, new DateTime(1990, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3@example.com", true, false, "ALRfQQPf+7OZnVYOUfJqNWjwRzcW3MYiTASn6/WkcuETnVCy7tyH47izVdvgn7Gomg==" },
                    { 4, new DateTime(1990, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4@example.com", true, false, "AOLs9j++/2NGvW6xOXJ6/J8C+zA6xJo6bdqPrGCKRkkXychlexQxSt51kspxjn+9cg==" },
                    { 5, new DateTime(1990, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "user5@example.com", true, false, "AGkrZWF3A+LcaNZ+MDaVavUDdp2xnM7SBjki0FXW3zYyS3mTv50O8zwJhii/toFtzA==" },
                    { 6, new DateTime(1990, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "user6@example.com", true, false, "AFJF1dZxF6g9gUe00IzKXL/q8gRFOgFhmpzyPmraTubjlUCXRiHKfq9/rete2406Sg==" },
                    { 7, new DateTime(1990, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "user7@example.com", true, false, "AAhMi6osv0mAob1F+D8Q1L3jK7GVcKkg9peetDP8BI0B4ionURaJ6lFbyojJ5mGyrg==" },
                    { 8, new DateTime(1990, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "user8@example.com", true, false, "AEcUAO2/vFuVyClGcRF2PNtod71GuwQFGHmYByKDBrBFjMCSwDWqNED8m40W0EfzEQ==" },
                    { 9, new DateTime(1990, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "user9@example.com", true, false, "AN8JsuvMPYpeldl0SJ8tWes2w+6E7q7Y6O09jsE6oxz+BC1WcQxTivd2YSPQidmk2g==" },
                    { 10, new DateTime(1990, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "user10@example.com", true, false, "AB8aoLjEKpWAVbLuV6IRadwmD7CCoqr7jBuS5cvezimwvkVQjh1CMleg54y9h0a+Wg==" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Users_UserId",
                table: "Todos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Users_UserId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Todos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Users_UserId",
                table: "Todos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
