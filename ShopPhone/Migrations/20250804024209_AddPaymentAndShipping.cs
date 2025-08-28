using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShopPhone.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentAndShipping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhuongThucGiaoHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhiGiaoHang = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HoatDong = table.Column<bool>(type: "bit", nullable: false),
                    ThuTu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucGiaoHang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoatDong = table.Column<bool>(type: "bit", nullable: false),
                    ThuTu = table.Column<int>(type: "int", nullable: false),
                    YeuCauTheTinDung = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToan", x => x.Id);
                }); 

            migrationBuilder.CreateTable(
                name: "TheTinDung",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoThe = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ChuThe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgayHetHan = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    LoaiThe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HanMuc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HoatDong = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheTinDung", x => x.Id);
                });

            
            migrationBuilder.CreateTable(
                name: "ThongTinGiaoHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonHangId = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TinhThanh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QuanHuyen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NgayGiaoDuKien = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaVanDon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThaiGiaoHang = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinGiaoHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongTinGiaoHang_DonHang_DonHangId",
                        column: x => x.DonHangId,
                        principalTable: "DonHang",
                        principalColumn: "DonHangId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PhuongThucGiaoHang",
                columns: new[] { "Id", "HoatDong", "MoTa", "PhiGiaoHang", "Ten", "ThuTu" },
                values: new object[,]
                {
                    { 1, true, "Giao hàng tận nơi trong vòng 2-3 ngày", 30000m, "Giao hàng tại nhà", 1 },
                    { 2, true, "Nhận hàng trực tiếp tại cửa hàng", 0m, "Giao hàng tại shop", 2 }
                });

            migrationBuilder.InsertData(
                table: "PhuongThucThanhToan",
                columns: new[] { "Id", "HoatDong", "Icon", "MoTa", "Ten", "ThuTu", "YeuCauTheTinDung" },
                values: new object[,]
                {
                    { 1, true, "fas fa-money-bill-wave", "Thanh toán bằng tiền mặt khi nhận hàng", "Tiền mặt", 1, false },
                    { 2, true, "fas fa-credit-card", "Thanh toán bằng thẻ tín dụng/ghi nợ", "Thẻ tín dụng", 2, true },
                    { 3, true, "fab fa-cc-mastercard", "Thanh toán qua ví điện tử MoMo", "MoMo", 3, false }
                });

            migrationBuilder.InsertData(
                table: "TheTinDung",
                columns: new[] { "Id", "CVV", "ChuThe", "HanMuc", "HoatDong", "LoaiThe", "NgayHetHan", "NgayTao", "SoThe" },
                values: new object[,]
                {
                    { 1, "123", "TEST USER", 1000000m, true, "Visa", "12/25", new DateTime(2025, 8, 4, 9, 42, 8, 39, DateTimeKind.Local).AddTicks(8793), "4111111111111111" },
                    { 2, "123", "TEST USER", 1000000m, true, "MasterCard", "12/25", new DateTime(2025, 8, 4, 9, 42, 8, 41, DateTimeKind.Local).AddTicks(3210), "5555555555554444" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TheTinDung");
           
            migrationBuilder.DropTable(
                name: "PhuongThucGiaoHang");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToan");
        }
    }
}