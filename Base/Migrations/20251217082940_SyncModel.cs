using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTManagement.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$7smxUjBMXM070y45IhBhAuCJg3muXsI9E7RtCRsZ7DydAdcpfYyOy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$FDifSSVuYg/r8m/bGHIYlOMTqju9Pd3poRTknQBZWj.4TO90XWjKe");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$lX/FWoOeYVOIyAyj3oL9X.uYp4no/f3x5UNxGEWEGnROsbQIQwtjq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$WeH2HX4EYqT3OPBiXXPQk.UQqbnJH7JeT.J.HI9MlGIMkP/CGLCiu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$r/sQ0yQhVBKxvHWvf8MD0eSycxmG.L7deV/1QBwz22jqoo05.CHpq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$hFn78zMyYldmh47PzSdpUOmi8SapGUwoL8UUPVrydD.3zu2ttG9pu");
        }
    }
}
