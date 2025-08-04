using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopPhone.Models;
using System.Security.Claims;

namespace ShopPhone.Controllers
{
    public class DonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult XacNhanDon()
        {
            // ✅ Kiểm tra đăng nhập
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            // ✅ Lấy ID người dùng từ claim
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                TempData["ThongBao"] = "❌ Lỗi: Không tìm thấy tên đăng nhập trong phiên đăng nhập.";
                return RedirectToAction("Index", "GioHang");
            }

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return RedirectToAction("Login", "Account");

            // ✅ Lấy giỏ hàng từ Db theo MaNguoiDung (kiểu string)
            var gioHang = _context.GioHangDb
                .Include(g => g.ChiTietGioHang)
                .FirstOrDefault(g => g.MaNguoiDung == userIdString); // ❗ kiểu string

            if (gioHang == null || !gioHang.ChiTietGioHang.Any())
                return RedirectToAction("Index", "GioHang");

            // ✅ Tính tổng tiền
            decimal tongTien = gioHang.ChiTietGioHang.Sum(item => item.SoLuong * item.DonGia.GetValueOrDefault());

            // ✅ Tạo đơn hàng
            var donHang = new DonHang
            {
                TaiKhoanId = userId, // Đây là khóa ngoại (int)
                TenDangNhap = User.Identity.Name,
                NgayDat = DateTime.Now,
                TongTien = tongTien,
                TrangThai = "Chờ xác nhận"
            };

            try
            {
                _context.DonHang.Add(donHang);
                _context.SaveChanges(); // => đơn hàng có DonHangId
            }
            catch (Exception ex)
            {
                return Content("❌ Lỗi khi lưu đơn hàng: " + ex.Message);
            }

            decimal tongTienDonHang = 0;

            foreach (var item in gioHang.ChiTietGioHang)
            {
                // ✅ Lấy giá gốc từ hàng hóa
                var hangHoa = _context.HangHoa.Find(item.MaHH);
                decimal giaGoc = hangHoa.GiaGoc;
                decimal giamGia = hangHoa.GiamGia ?? 0;
                decimal giaSauGiam = giaGoc * (1 - giamGia / 100);

                // ✅ Tính bảo hành
                decimal tienBH = 0;
                if (item.BaoHanh1) tienBH += 990_000;
                if (item.BaoHanh2) tienBH += 1_300_000;

                // ✅ Giá cuối cùng mỗi đơn vị
                decimal donGia = giaSauGiam + tienBH;

                // ✅ Thành tiền
                decimal thanhTien = donGia * item.SoLuong;
                tongTienDonHang += thanhTien;

                // ✅ Lưu chi tiết
                var chiTiet = new ChiTietDonHang
                {
                    DonHangId = donHang.DonHangId,
                    MaHH = item.MaHH,
                    SoLuong = item.SoLuong,
                    DonGia = donGia,
                    DonGiaGoc = giaGoc,
                    GiamGia = giamGia,
                    BaoHanh1 = item.BaoHanh1,
                    BaoHanh2 = item.BaoHanh2,
                    ThanhTien = thanhTien // ✅ CÓ dòng này!
                };

                _context.ChiTietDonHang.Add(chiTiet);
            }

            donHang.TongTien = tongTienDonHang;
            _context.SaveChanges();

            // ✅ Xóa giỏ hàng và chi tiết
            _context.GioHangChiTietDb.RemoveRange(gioHang.ChiTietGioHang);
            _context.GioHangDb.Remove(gioHang);
            _context.SaveChanges();

            // ✅ Điều hướng đến trang "ThanhCong"
            return RedirectToAction("ThanhCong", new { id = donHang.DonHangId });
        }

        public IActionResult ThanhCong(int id)
        {
            var chiTiet = _context.ChiTietDonHang
                .Include(c => c.HangHoa)
                .Where(c => c.DonHangId == id)
                .ToList();

            var donHang = _context.DonHang.FirstOrDefault(d => d.DonHangId == id);

            ViewBag.DonHang = donHang;
            return View(chiTiet);
        }

        private List<GioHangItem> LayGioHangTuSession()
        {
            var sessionData = HttpContext.Session.GetString("GioHang");
            if (string.IsNullOrEmpty(sessionData))
                return new List<GioHangItem>();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<GioHangItem>>(sessionData);
        }
    }
}