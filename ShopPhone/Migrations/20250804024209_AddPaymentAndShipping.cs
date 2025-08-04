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
                name: "GioHangDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenDangNhap = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangHoa",
                columns: table => new
                {
                    MaHH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenAlias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaLoai = table.Column<int>(type: "int", nullable: false),
                    MoTaDonVi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Hinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgaySX = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLanXem = table.Column<int>(type: "int", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNCC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DanhGia = table.Column<double>(type: "float", nullable: true),
                    HinhMoHop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhThucTe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangHoa", x => x.MaHH);
                });

            migrationBuilder.CreateTable(
                name: "LienHe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayGui = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LienHe", x => x.Id);
                });

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
                name: "TaiKhoan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.Id);
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
                name: "GioHangChiTietDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GioHangDbId = table.Column<int>(type: "int", nullable: false),
                    MaHH = table.Column<int>(type: "int", nullable: false),
                    BaoHanh1 = table.Column<bool>(type: "bit", nullable: false),
                    BaoHanh2 = table.Column<bool>(type: "bit", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiamGia = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    GoiBaoHanh = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangChiTietDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GioHangChiTietDb_GioHangDb_GioHangDbId",
                        column: x => x.GioHangDbId,
                        principalTable: "GioHangDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GioHangChiTietDb_HangHoa_MaHH",
                        column: x => x.MaHH,
                        principalTable: "HangHoa",
                        principalColumn: "MaHH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    DonHangId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DaXacNhan = table.Column<bool>(type: "bit", nullable: false),
                    TaiKhoanId = table.Column<int>(type: "int", nullable: false),
                    TenDangNhap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhuongThucThanhToanId = table.Column<int>(type: "int", nullable: true),
                    MaGiaoDich = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThaiThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhuongThucGiaoHangId = table.Column<int>(type: "int", nullable: true),
                    PhiGiaoHang = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongTienSauPhiGiaoHang = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.DonHangId);
                    table.ForeignKey(
                        name: "FK_DonHang_PhuongThucGiaoHang_PhuongThucGiaoHangId",
                        column: x => x.PhuongThucGiaoHangId,
                        principalTable: "PhuongThucGiaoHang",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DonHang_PhuongThucThanhToan_PhuongThucThanhToanId",
                        column: x => x.PhuongThucThanhToanId,
                        principalTable: "PhuongThucThanhToan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonHangId = table.Column<int>(type: "int", nullable: false),
                    MaHH = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DonGiaGoc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BaoHanh1 = table.Column<bool>(type: "bit", nullable: false),
                    BaoHanh2 = table.Column<bool>(type: "bit", nullable: false),
                    GiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonHang_DonHangId",
                        column: x => x.DonHangId,
                        principalTable: "DonHang",
                        principalColumn: "DonHangId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_HangHoa_MaHH",
                        column: x => x.MaHH,
                        principalTable: "HangHoa",
                        principalColumn: "MaHH",
                        onDelete: ReferentialAction.Cascade);
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
                    { 1, "123", "TEST USER", 79228162514264337593543950335m, true, "Visa", "12/25", new DateTime(2025, 8, 4, 9, 42, 8, 39, DateTimeKind.Local).AddTicks(8793), "4111111111111111" },
                    { 2, "123", "TEST USER", 79228162514264337593543950335m, true, "MasterCard", "12/25", new DateTime(2025, 8, 4, 9, 42, 8, 41, DateTimeKind.Local).AddTicks(3210), "5555555555554444" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_DonHangId",
                table: "ChiTietDonHang",
                column: "DonHangId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaHH",
                table: "ChiTietDonHang",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_PhuongThucGiaoHangId",
                table: "DonHang",
                column: "PhuongThucGiaoHangId");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_PhuongThucThanhToanId",
                table: "DonHang",
                column: "PhuongThucThanhToanId");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTietDb_GioHangDbId",
                table: "GioHangChiTietDb",
                column: "GioHangDbId");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTietDb_MaHH",
                table: "GioHangChiTietDb",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinGiaoHang_DonHangId",
                table: "ThongTinGiaoHang",
                column: "DonHangId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "GioHangChiTietDb");

            migrationBuilder.DropTable(
                name: "LienHe");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "TheTinDung");

            migrationBuilder.DropTable(
                name: "ThongTinGiaoHang");

            migrationBuilder.DropTable(
                name: "GioHangDb");

            migrationBuilder.DropTable(
                name: "HangHoa");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "PhuongThucGiaoHang");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToan");
        }
    }
}