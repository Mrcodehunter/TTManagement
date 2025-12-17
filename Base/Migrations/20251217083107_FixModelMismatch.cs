using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixModelMismatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Vti30hPqM1ET7I9/egw0Deh/kESzzwf3SJjEmNea7nzHb/6GBuT.a");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$g/FeuGg1RTZj7CU2Db2fTetNFz449ZSZjSNqgQ4c1WZ1aqsNylwPq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$ctx0N5zUpW4iubir0Ld2DuQ/PdGaD1PRDdSlLlfpz/IzerVWD.oy6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
