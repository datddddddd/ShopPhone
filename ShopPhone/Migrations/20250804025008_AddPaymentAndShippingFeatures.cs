using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPhone.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentAndShippingFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HanMuc", "NgayTao" },
                values: new object[] { 999999m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HanMuc", "NgayTao" },
                values: new object[] { 999999m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HanMuc", "NgayTao" },
                values: new object[] { 79228162514264337593543950335m, new DateTime(2025, 8, 4, 9, 42, 8, 39, DateTimeKind.Local).AddTicks(8793) });

            migrationBuilder.UpdateData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HanMuc", "NgayTao" },
                values: new object[] { 79228162514264337593543950335m, new DateTime(2025, 8, 4, 9, 42, 8, 41, DateTimeKind.Local).AddTicks(3210) });
        }
    }
}