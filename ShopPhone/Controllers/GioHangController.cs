using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopPhone.Models;
using System.Security.Claims;
using System.Text;

namespace ShopPhone.Controllers
{
    [Authorize]
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GioHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("/gio-hang")]
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity?.Name;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Account");
            }

            var gioHang = await _context.GioHangDb
                .Include(x => x.ChiTietGioHang)
                .ThenInclude(x => x.HangHoa)
                .FirstOrDefaultAsync(x => x.TenDangNhap == userName);

            if (gioHang == null)
            {
                gioHang = new GioHangDb
                {
                    TenDangNhap = userName,
                    NgayTao = DateTime.Now,
                    MaNguoiDung = userId,
                    ChiTietGioHang = new List<GioHangChiTietDb>()
                };
                _context.GioHangDb.Add(gioHang);
                await _context.SaveChangesAsync();
            }

            return View(gioHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemVaoGio([FromBody] ThemVaoGioModel model)
        {
            var userName = User.Identity.Name;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập." });
            }

            // ✔ Tìm giỏ hàng theo userId chứ KHÔNG chỉ theo userName
            var gioHang = await _context.GioHangDb.FirstOrDefaultAsync(x => x.MaNguoiDung == userId);

            if (gioHang == null)
            {
                gioHang = new GioHangDb
                {
                    MaNguoiDung = userId,      // ✔ Thêm dòng này
                    TenDangNhap = userName,   // ✔ Cũng giữ dòng này nếu bạn vẫn hiển thị tên
                    NgayTao = DateTime.Now
                };
                _context.GioHangDb.Add(gioHang);
                await _context.SaveChangesAsync();
            }

            var chiTiet = await _context.GioHangChiTietDb
                .FirstOrDefaultAsync(x =>
    x.GioHangDbId == gioHang.Id &&
    x.MaHH == model.MaHH &&
    x.BaoHanh1 == model.BaoHanh1 &&
    x.BaoHanh2 == model.BaoHanh2
);

            if (chiTiet != null)
            {
                chiTiet.SoLuong += model.SoLuong;
                chiTiet.BaoHanh1 = model.BaoHanh1;
                chiTiet.BaoHanh2 = model.BaoHanh2;
            }
            else
            {
                var hang = await _context.HangHoa.FindAsync(model.MaHH);
                chiTiet = new GioHangChiTietDb
                {
                    GioHangDbId = gioHang.Id,
                    MaHH = model.MaHH,
                    SoLuong = model.SoLuong,
                    DonGia = hang.DonGia ?? 0,
                    GiamGia = 0,
                    BaoHanh1 = model.BaoHanh1,
                    BaoHanh2 = model.BaoHanh2
                };
                _context.GioHangChiTietDb.Add(chiTiet);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Đã thêm vào giỏ hàng!" });
        }

        [HttpPost]
        public async Task<IActionResult> Xoa(int id)
        {
            var chiTiet = await _context.GioHangChiTietDb.FindAsync(id);
            if (chiTiet != null)
            {
                _context.GioHangChiTietDb.Remove(chiTiet);
                await _context.SaveChangesAsync();

                // 👇 Gửi thông báo về View
                TempData["ThongBao"] = "✅ Đã xóa sản phẩm khỏi giỏ hàng!";
            }
            else
            {
                TempData["ThongBao"] = "❌ Không tìm thấy sản phẩm cần xóa.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatSoLuong([FromForm] CapNhatSoLuongVM model)
        {
            // 1) Tìm dòng chi tiết
            var chiTiet = await _context.GioHangChiTietDb
                                        .Include(ct => ct.HangHoa)
                                        .FirstOrDefaultAsync(ct => ct.Id == model.Id);
            if (chiTiet == null) return Json(new { success = false });

            // 2) Cập nhật số lượng
            chiTiet.SoLuong = model.SoLuong;
            await _context.SaveChangesAsync();

            // 3) Tải lại giỏ + hàng
            var gio = await _context.GioHangDb
                .Include(g => g.ChiTietGioHang)
                    .ThenInclude(ct => ct.HangHoa)
                .FirstAsync(g => g.Id == chiTiet.GioHangDbId);

            // ---------- TÍNH TỔNG ----------
            var tongTien = gio.ChiTietGioHang.Sum(ct => (ct.DonGia ?? 0) * ct.SoLuong);
            var tongGiam = gio.ChiTietGioHang.Sum(ct =>
                ((ct.HangHoa.DonGia ?? 0) * (ct.HangHoa.GiamGia ?? 0) / 100) * ct.SoLuong);
            var canThanhToan = tongTien - tongGiam;

            // ---------- GHÉP HTML khuyến mãi + bảo hành ----------
            var sb = new StringBuilder();
            foreach (var ct in gio.ChiTietGioHang)
            {
                // 3.1 Giảm % (nếu có)
                var ptGiam = ct.HangHoa.GiamGia ?? 0;
                if (ptGiam > 0)
                {
                    var giamTien = (ct.HangHoa.DonGia ?? 0) * ptGiam / 100 * ct.SoLuong;
                    sb.Append($"<li><strong>{ct.HangHoa.TenHH}:</strong> Giảm {ptGiam:0.#}% x {ct.SoLuong} = {giamTien:N0} đ</li>");
                }

                // 3.2 Bảo hành (tiền BH = DonGia – giá SP giảm)
                var tienBH = (ct.DonGia ?? 0) - (ct.HangHoa.DonGia ?? 0);
                if (tienBH > 0)
                {
                    sb.Append($"<li><strong>{ct.HangHoa.TenHH}:</strong> Bảo hành +{tienBH:N0} đ</li>");
                }
            }

            // 4) Trả JSON
            return Json(new
            {
                success = true,
                tongTien,
                tongGiam,
                canThanhToan,
                htmlKM = sb.ToString()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatBaoHanh([FromBody] CapNhatBaoHanh model)
        {
            var chiTiet = await _context.GioHangChiTietDb
                .Include(x => x.HangHoa)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (chiTiet == null) return Json(new { success = false });

            // Gán lại thông tin bảo hành vào chi tiết giỏ hàng
            chiTiet.BaoHanh1 = model.BaoHanh1;
            chiTiet.BaoHanh2 = model.BaoHanh2;

            // Tính tiền bảo hành
            decimal tienBH = 0;
            if (model.BaoHanh1) tienBH += 990_000;
            if (model.BaoHanh2) tienBH += 1_300_000;

            // Gán lại đơn giá
            var donGiaSanPham = chiTiet.HangHoa.DonGia ?? 0;
            chiTiet.SoLuong = model.SoLuong;
            chiTiet.DonGia = donGiaSanPham + tienBH;

            await _context.SaveChangesAsync();

            // Tính lại toàn bộ giỏ hàng
            var gio = await _context.GioHangDb
                .Include(x => x.ChiTietGioHang)
                .ThenInclude(x => x.HangHoa)
                .FirstOrDefaultAsync(x => x.Id == chiTiet.GioHangDbId);

            var tongTien = gio?.ChiTietGioHang?.Sum(x => (x.DonGia ?? 0) * x.SoLuong) ?? 0;

            var tongGiam = gio?.ChiTietGioHang?.Sum(x => ((x.HangHoa?.DonGia ?? 0) * (x.HangHoa?.GiamGia ?? 0) / 100) * x.SoLuong) ?? 0;

            var canThanhToan = tongTien - tongGiam;

            // Danh sách khuyến mãi và bảo hành
            var htmlKM = new StringBuilder();
            if (gio?.ChiTietGioHang != null)
            {
                foreach (var ct in gio.ChiTietGioHang)
                {
                    var giam = ct.HangHoa?.GiamGia ?? 0;
                    var donGiaGoc = ct.HangHoa?.DonGia ?? 0;
                    var donGiaThucTe = ct.DonGia ?? 0;

                    if (giam > 0)
                    {
                        var giamTien = donGiaGoc * giam / 100 * ct.SoLuong;
                        htmlKM.Append($"<li><strong>{ct.HangHoa?.TenHH}:</strong> Giảm {giam:0.#}% x {ct.SoLuong} = {giamTien:N0} đ</li>");
                    }

                    var tienBH1SP = donGiaThucTe - donGiaGoc;
                    var tienBHTong = tienBH1SP * ct.SoLuong;

                    if (tienBH1SP > 0)
                    {
                        htmlKM.Append($@"
        <li><strong>{ct.HangHoa?.TenHH}:</strong>
        Bảo hành +{tienBH1SP:N0} đ × {ct.SoLuong} = {tienBHTong:N0} đ</li>
    ");
                    }
                }
            }

            return Json(new
            {
                success = true,
                tongTien,
                tongGiam,
                canThanhToan,
                htmlKM = htmlKM.ToString()
            });
        }
    }
}