using Microsoft.EntityFrameworkCore;
using ShopPhone.Models; // chứa class Product (hoặc SanPham)

namespace ShopPhone.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<TaiKhoan> TaiKhoan { get; set; } = null;
        public DbSet<LienHe> LienHe { get; set; }
        public DbSet<HangHoa> HangHoa { get; set; }
        public DbSet<GioHangDb> GioHangDb { get; set; }
        public DbSet<GioHangChiTietDb> GioHangChiTietDb { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }

    }
}
