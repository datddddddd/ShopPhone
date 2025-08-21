using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShopPhone.Migrations
{
    /// <inheritdoc />
    public partial class FixDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiGiaoHang",
                table: "ThongTinGiaoHang",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LoaiThe",
                table: "TheTinDung",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VaiTro",
                table: "TaiKhoan",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MatKhau",
                table: "TaiKhoan",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "HoTen",
                table: "TaiKhoan",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "TaiKhoan",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MoTaDonVi",
                table: "HangHoa",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "DonHang",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ViDienTu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChuVi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LoaiVi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaiKhoanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViDienTu", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 1,
                column: "HanMuc",
                value: 1000000m);

            migrationBuilder.UpdateData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 2,
                column: "HanMuc",
                value: 1000000m);

            migrationBuilder.InsertData(
                table: "TheTinDung",
                columns: new[] { "Id", "CVV", "ChuThe", "HanMuc", "HoatDong", "LoaiThe", "NgayHetHan", "NgayTao", "SoThe" },
                values: new object[,]
                {
                    { 3, "456", "TEST USER 2", 1000000m, true, "Visa", "11/26", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "4000000000000002" },
                    { 4, "789", "TEST USER 3", 1000000m, true, "MasterCard", "10/27", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "5105105105105100" },
                    { 5, "321", "TEST USER 4", 1000000m, true, "Discover", "09/28", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "6011000990139424" },
                    { 6, "654", "TEST USER 5", 1000000m, true, "JCB", "08/29", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3530111333300000" }
                });

            migrationBuilder.InsertData(
                table: "ViDienTu",
                columns: new[] { "Id", "LoaiVi", "SoDienThoai", "TaiKhoanId", "TenChuVi" },
                values: new object[] { 1, "Momo", "0909123456", 1, "Test Momo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViDienTu");

            migrationBuilder.DeleteData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiGiaoHang",
                table: "ThongTinGiaoHang",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoaiThe",
                table: "TheTinDung",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VaiTro",
                table: "TaiKhoan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MatKhau",
                table: "TaiKhoan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HoTen",
                table: "TaiKhoan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "TaiKhoan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MoTaDonVi",
                table: "HangHoa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "DonHang",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 1,
                column: "HanMuc",
                value: 1000000m);

            migrationBuilder.UpdateData(
                table: "TheTinDung",
                keyColumn: "Id",
                keyValue: 2,
                column: "HanMuc",
                value: 1000000m);
        }
    }
}
