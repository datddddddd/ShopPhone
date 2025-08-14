using Microsoft.EntityFrameworkCore;

namespace ShopPhone.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TaiKhoan> TaiKhoan { get; set; } = null!;
        public DbSet<LienHe> LienHe { get; set; } = null!;
        public DbSet<HangHoa> HangHoa { get; set; } = null!;
        public DbSet<GioHangDb> GioHangDb { get; set; } = null!;
        public DbSet<GioHangChiTietDb> GioHangChiTietDb { get; set; } = null!;
        public DbSet<DonHang> DonHang { get; set; } = null!;
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; } = null!;

        // Thanh toán và giao hàng
        public DbSet<PhuongThucThanhToan> PhuongThucThanhToan { get; set; } = null!;

        public DbSet<PhuongThucGiaoHang> PhuongThucGiaoHang { get; set; } = null!;
        public DbSet<TheTinDung> TheTinDung { get; set; } = null!;
        public DbSet<ThongTinGiaoHang> ThongTinGiaoHang { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data cho phương thức thanh toán
            modelBuilder.Entity<PhuongThucThanhToan>().HasData(
                new PhuongThucThanhToan
                {
                    Id = 1,
                    Ten = "Tiền mặt",
                    MoTa = "Thanh toán bằng tiền mặt khi nhận hàng",
                    Icon = "fas fa-money-bill-wave",
                    ThuTu = 1,
                    YeuCauTheTinDung = false
                },
                new PhuongThucThanhToan
                {
                    Id = 2,
                    Ten = "Thẻ tín dụng",
                    MoTa = "Thanh toán bằng thẻ tín dụng/ghi nợ",
                    Icon = "fas fa-credit-card",
                    ThuTu = 2,
                    YeuCauTheTinDung = true
                },
                new PhuongThucThanhToan
                {
                    Id = 3,
                    Ten = "MoMo",
                    MoTa = "Thanh toán qua ví điện tử MoMo",
                    Icon = "fab fa-cc-mastercard",
                    ThuTu = 3,
                    YeuCauTheTinDung = false
                }
            );

            // Seed data cho phương thức giao hàng
            modelBuilder.Entity<PhuongThucGiaoHang>().HasData(
                new PhuongThucGiaoHang
                {
                    Id = 1,
                    Ten = "Giao hàng tại nhà",
                    MoTa = "Giao hàng tận nơi trong vòng 2-3 ngày",
                    PhiGiaoHang = 30000,
                    ThuTu = 1
                },
                new PhuongThucGiaoHang
                {
                    Id = 2,
                    Ten = "Giao hàng tại shop",
                    MoTa = "Nhận hàng trực tiếp tại cửa hàng",
                    PhiGiaoHang = 0,
                    ThuTu = 2
                }
            );

            // Seed data cho thẻ tín dụng test
            modelBuilder.Entity<TheTinDung>().HasData(
                new TheTinDung
                {
                    Id = 1,
                    SoThe = "4111111111111111",
                    ChuThe = "TEST USER",
                    NgayHetHan = "12/25",
                    CVV = "123",
                    LoaiThe = "Visa",
                    HanMuc = 999999,
                    HoatDong = true
                },
                new TheTinDung
                {
                    Id = 2,
                    SoThe = "5555555555554444",
                    ChuThe = "TEST USER",
                    NgayHetHan = "12/25",
                    CVV = "123",
                    LoaiThe = "MasterCard",
                    HanMuc = 999999,
                    HoatDong = true
                }
            );
        }
    }
}