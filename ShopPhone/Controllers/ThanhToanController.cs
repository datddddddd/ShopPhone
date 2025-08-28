using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopPhone.Models;
using System.Security.Claims;

namespace ShopPhone.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThanhToanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị trang thanh toán
        public async Task<IActionResult> Index()
        {
            try
            {
                if (!User.Identity?.IsAuthenticated ?? true)
                    return RedirectToAction("Login", "Account");

                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdString))
                    return RedirectToAction("Login", "Account");

                // Lấy giỏ hàng
                var gioHang = await _context.GioHangDb
                    .Include(g => g.ChiTietGioHang)
                    .ThenInclude(c => c.HangHoa)
                    .FirstOrDefaultAsync(g => g.MaNguoiDung == userIdString);

                if (gioHang == null || !gioHang.ChiTietGioHang.Any())
                {
                    TempData["ThongBao"] = "Giỏ hàng trống!";
                    return RedirectToAction("Index", "GioHang");
                }

                // Tính tổng tiền với validation
                decimal tongTien = 0;
                foreach (var item in gioHang.ChiTietGioHang)
                {
                    if (item.HangHoa == null)
                    {
                        TempData["Loi"] = "Có sản phẩm trong giỏ hàng không tồn tại!";
                        return RedirectToAction("Index", "GioHang");
                    }

                    var hangHoa = item.HangHoa;
                    decimal giaGoc = hangHoa.DonGia ?? 0;
                    decimal giamGia = Math.Max(0, Math.Min(100, hangHoa.GiamGia ?? 0)); // Giới hạn giảm giá 0-100%
                    decimal giaSauGiam = giaGoc * (1 - giamGia / 100);

                    decimal tienBH = 0;
                    if (item.BaoHanh1) tienBH += 990_000;
                    if (item.BaoHanh2) tienBH += 1_300_000;

                    decimal donGia = giaSauGiam + tienBH;
                    tongTien += donGia * Math.Max(1, item.SoLuong); // Đảm bảo số lượng >= 1
                }

                var viewModel = new ThanhToanViewModel
                {
                    TongTien = tongTien,
                    DanhSachGiaoHang = await _context.PhuongThucGiaoHang
                        .Where(p => p.HoatDong)
                        .OrderBy(p => p.ThuTu)
                        .ToListAsync(),
                    DanhSachThanhToan = await _context.PhuongThucThanhToan
                        .Where(p => p.HoatDong)
                        .OrderBy(p => p.ThuTu)
                        .ToListAsync()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Loi"] = $"Có lỗi xảy ra khi tải trang thanh toán: {ex.Message}";
                return RedirectToAction("Index", "GioHang");
            }
        }

        // Xử lý thanh toán
        [HttpPost]
        public async Task<IActionResult> XuLyThanhToan(ThanhToanViewModel model)
        {
            try
            {
                if (!User.Identity?.IsAuthenticated ?? true)
                    return RedirectToAction("Login", "Account");

                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                    return RedirectToAction("Login", "Account");

                if (!ModelState.IsValid)
                {
                    await LoadDropdownData(model);
                    return View("Index", model);
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.HoTen) || string.IsNullOrWhiteSpace(model.SoDienThoai) ||
                    string.IsNullOrWhiteSpace(model.DiaChi))
                {
                    ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin giao hàng!");
                    await LoadDropdownData(model);
                    return View("Index", model);
                }

                // Lấy giỏ hàng
                var gioHang = await _context.GioHangDb
                    .Include(g => g.ChiTietGioHang)
                    .ThenInclude(c => c.HangHoa)
                    .FirstOrDefaultAsync(g => g.MaNguoiDung == userIdString);

                if (gioHang == null || !gioHang.ChiTietGioHang.Any())
                {
                    TempData["ThongBao"] = "Giỏ hàng trống!";
                    return RedirectToAction("Index", "GioHang");
                }

                // Lấy thông tin phương thức giao hàng và thanh toán
                var phuongThucGiaoHang = await _context.PhuongThucGiaoHang.FindAsync(model.PhuongThucGiaoHangId);
                var phuongThucThanhToan = await _context.PhuongThucThanhToan.FindAsync(model.PhuongThucThanhToanId);

                if (phuongThucGiaoHang == null || phuongThucThanhToan == null)
                {
                    TempData["ThongBao"] = "Phương thức không hợp lệ!";
                    return RedirectToAction("Index");
                }

                // Tính tổng tiền
                decimal tongTien = 0;
                foreach (var item in gioHang.ChiTietGioHang)
                {
                    var hangHoa = item.HangHoa;
                    decimal giaGoc = hangHoa.DonGia ?? 0; // Sử dụng DonGia thay vì GiaGoc
                    decimal giamGia = hangHoa.GiamGia ?? 0;
                    decimal giaSauGiam = giaGoc * (1 - giamGia / 100);

                    decimal tienBH = 0;
                    if (item.BaoHanh1) tienBH += 990_000;
                    if (item.BaoHanh2) tienBH += 1_300_000;

                    decimal donGia = giaSauGiam + tienBH;
                    tongTien += donGia * item.SoLuong;
                }

                decimal phiGiaoHang = phuongThucGiaoHang.PhiGiaoHang;
                decimal tongTienSauPhiGiaoHang = tongTien + phiGiaoHang;

                // Kiểm tra thẻ tín dụng nếu cần
                if (phuongThucThanhToan.YeuCauTheTinDung)
                {
                    if (string.IsNullOrEmpty(model.SoThe) || string.IsNullOrEmpty(model.ChuThe) ||
                        string.IsNullOrEmpty(model.NgayHetHan) || string.IsNullOrEmpty(model.CVV))
                    {
                        ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin thẻ tín dụng!");
                        model.DanhSachGiaoHang = _context.PhuongThucGiaoHang.Where(p => p.HoatDong).OrderBy(p => p.ThuTu).ToList();
                        model.DanhSachThanhToan = _context.PhuongThucThanhToan.Where(p => p.HoatDong).OrderBy(p => p.ThuTu).ToList();
                        return View("Index", model);
                    }

                    // Kiểm tra thẻ test
                    var theTinDung = _context.TheTinDung.FirstOrDefault(t =>
                        t.SoThe == model.SoThe &&
                        t.ChuThe == model.ChuThe &&
                        t.NgayHetHan == model.NgayHetHan &&
                        t.CVV == model.CVV &&
                        t.HoatDong);

                    if (theTinDung == null)
                    {
                        ModelState.AddModelError("", "Thông tin thẻ tín dụng không hợp lệ!");
                        model.DanhSachGiaoHang = _context.PhuongThucGiaoHang.Where(p => p.HoatDong).OrderBy(p => p.ThuTu).ToList();
                        model.DanhSachThanhToan = _context.PhuongThucThanhToan.Where(p => p.HoatDong).OrderBy(p => p.ThuTu).ToList();
                        return View("Index", model);
                    }
                }
                // Kiểm tra ví MoMo nếu chọn MoMo
                if (phuongThucThanhToan.Ten == "MoMo")
                {
                    if (string.IsNullOrEmpty(model.SoDienThoaiVi) || string.IsNullOrEmpty(model.TenChuVi))
                    {
                        ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin ví MoMo!");
                        model.DanhSachGiaoHang = _context.PhuongThucGiaoHang.Where(p => p.HoatDong).OrderBy(p => p.ThuTu).ToList();
                        model.DanhSachThanhToan = _context.PhuongThucThanhToan.Where(p => p.HoatDong).OrderBy(p => p.ThuTu).ToList();
                        return View("Index", model);
                    }
                    var vi = _context.ViDienTu.FirstOrDefault(v => v.SoDienThoai == model.SoDienThoaiVi && v.TenChuVi == model.TenChuVi);
                    if (vi == null)
                    {
                        ModelState.AddModelError("", "Thông tin ví MoMo không hợp lệ!");
                        model.DanhSachGiaoHang = _context.PhuongThucGiaoHang.Where(p => p.HoatDong).OrderBy(p => p.ThuTu).ToList();
                        model.DanhSachThanhToan = _context.PhuongThucThanhToan.Where(p => p.HoatDong).OrderBy(p => p.ThuTu).ToList();
                        return View("Index", model);
                    }
                }

                try
                {
                    // Tạo đơn hàng
                    var donHang = new DonHang
                    {
                        TaiKhoanId = userId,
                        TenDangNhap = User.Identity.Name,
                        NgayDat = DateTime.Now,
                        TongTien = tongTien,
                        TrangThai = "Chờ xác nhận",
                        PhuongThucThanhToanId = model.PhuongThucThanhToanId,
                        PhuongThucGiaoHangId = model.PhuongThucGiaoHangId,
                        PhiGiaoHang = phiGiaoHang,
                        TongTienSauPhiGiaoHang = tongTienSauPhiGiaoHang,
                        MaGiaoDich = GenerateTransactionId(),
                        TrangThaiThanhToan = "Đã thanh toán",
                        NgayThanhToan = DateTime.Now
                    };

                    _context.DonHang.Add(donHang);
                    _context.SaveChanges();

                    // Tạo chi tiết đơn hàng
                    foreach (var item in gioHang.ChiTietGioHang)
                    {
                        var hangHoa = item.HangHoa;
                        decimal giaGoc = hangHoa.DonGia ?? 0; // Sử dụng DonGia thay vì GiaGoc
                        decimal giamGia = hangHoa.GiamGia ?? 0;
                        decimal giaSauGiam = giaGoc * (1 - giamGia / 100);

                        decimal tienBH = 0;
                        if (item.BaoHanh1) tienBH += 990_000;
                        if (item.BaoHanh2) tienBH += 1_300_000;

                        decimal donGia = giaSauGiam + tienBH;
                        decimal thanhTien = donGia * item.SoLuong;

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
                            ThanhTien = thanhTien
                        };

                        _context.ChiTietDonHang.Add(chiTiet);
                    }

                    // Tạo thông tin giao hàng
                    var thongTinGiaoHang = new ThongTinGiaoHang
                    {
                        DonHangId = donHang.DonHangId,
                        HoTen = model.HoTen,
                        SoDienThoai = model.SoDienThoai,
                        DiaChi = model.DiaChi,
                        TinhThanh = model.TinhThanh,
                        QuanHuyen = model.QuanHuyen,
                        GhiChu = model.GhiChu,
                        NgayGiaoDuKien = DateTime.Now.AddDays(phuongThucGiaoHang.Ten.Contains("tại nhà") ? 3 : 1),
                        TrangThaiGiaoHang = "Chờ xử lý"
                    };

                    _context.ThongTinGiaoHang.Add(thongTinGiaoHang);

                    // Xóa giỏ hàng
                    _context.GioHangChiTietDb.RemoveRange(gioHang.ChiTietGioHang);
                    _context.GioHangDb.Remove(gioHang);

                    _context.SaveChanges();

                    TempData["ThanhCong"] = $"Đặt hàng thành công! Mã đơn hàng: {donHang.DonHangId}";
                    return RedirectToAction("ThanhCong", new { id = donHang.DonHangId });
                }
                catch (Exception ex)
                {
                    TempData["Loi"] = $"Lỗi khi xử lý đơn hàng: {ex.Message}";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Loi"] = $"Lỗi khi xử lý đơn hàng: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
        // Trang thanh công
        public IActionResult ThanhCong(int id)
        {
            var donHang = _context.DonHang
                .Include(d => d.PhuongThucThanhToan)
                .Include(d => d.PhuongThucGiaoHang)
                .Include(d => d.ThongTinGiaoHang)
                .Include(d => d.ChiTietDonHang)
                .ThenInclude(c => c.HangHoa)
                .FirstOrDefault(d => d.DonHangId == id);

            if (donHang == null)
            {
                TempData["Loi"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction("Index", "Home");
            }

            return View(donHang);
        }

        // API để tính phí giao hàng
        [HttpPost]
        public IActionResult TinhPhiGiaoHang(int phuongThucGiaoHangId)
        {
            var phuongThuc = _context.PhuongThucGiaoHang.Find(phuongThucGiaoHangId);
            if (phuongThuc == null)
                return Json(new { success = false, message = "Phương thức không tồn tại" });

            return Json(new { success = true, phiGiaoHang = phuongThuc.PhiGiaoHang });
        }

        // Trang thanh toán MoMo
        public IActionResult ThanhToanMoMo(ThanhToanViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            // Validate thông tin cơ bản
            if (model.TongTien <= 0)
            {
                TempData["Loi"] = "Thông tin đơn hàng không hợp lệ!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // Xử lý thanh toán MoMo
        [HttpPost]
        public IActionResult XuLyThanhToanMoMo(ThanhToanViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            // Validate thông tin MoMo
            if (string.IsNullOrEmpty(model.SoDienThoaiVi) || string.IsNullOrEmpty(model.TenChuVi))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin ví MoMo!");
                return View("ThanhToanMoMo", model);
            }

            // Kiểm tra ví MoMo test
            var vi = _context.ViDienTu.FirstOrDefault(v =>
                v.SoDienThoai == model.SoDienThoaiVi &&
                v.TenChuVi == model.TenChuVi);

            if (vi == null)
            {
                ModelState.AddModelError("", "Thông tin ví MoMo không hợp lệ!");
                return View("ThanhToanMoMo", model);
            }

            // Simulate MoMo payment processing
            TempData["ThanhToanThanhCong"] = "Thanh toán MoMo thành công!";
            return RedirectToAction("XacNhanThanhToan", model);
        }

        // Trang thanh toán thẻ tín dụng
        public IActionResult ThanhToanTheTinDung(ThanhToanViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            // Validate thông tin cơ bản
            if (model.TongTien <= 0)
            {
                TempData["Loi"] = "Thông tin đơn hàng không hợp lệ!";
                return RedirectToAction("Index");
            }

            // Load danh sách ngân hàng
            ViewBag.DanhSachNganHang = _context.NganHang.Where(n => n.HoatDong).OrderBy(n => n.ThuTu).ToList();

            return View(model);
        }

        // Xử lý thanh toán thẻ tín dụng
        [HttpPost]
        public IActionResult XuLyThanhToanTheTinDung(ThanhToanViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            // Validate thông tin thẻ
            if (string.IsNullOrEmpty(model.SoThe) || string.IsNullOrEmpty(model.ChuThe) ||
                string.IsNullOrEmpty(model.NgayHetHan) || string.IsNullOrEmpty(model.CVV))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin thẻ tín dụng!");
                return View("ThanhToanTheTinDung", model);
            }

            // Kiểm tra thẻ test (loại bỏ dấu cách trong số thẻ)
            var soTheClean = model.SoThe?.Replace(" ", "");
            var theTinDung = _context.TheTinDung.FirstOrDefault(t =>
                t.SoThe == soTheClean &&
                t.ChuThe == model.ChuThe &&
                t.NgayHetHan == model.NgayHetHan &&
                t.CVV == model.CVV &&
                t.HoatDong);

            if (theTinDung == null)
            {
                ModelState.AddModelError("", "Thông tin thẻ tín dụng không hợp lệ!");
                return View("ThanhToanTheTinDung", model);
            }

            // Tạo đơn hàng thực sự
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdString, out int userId))
                    return RedirectToAction("Login", "Account");

                // Lấy giỏ hàng
                var gioHang = _context.GioHangDb
                    .Include(g => g.ChiTietGioHang)
                    .ThenInclude(c => c.HangHoa)
                    .FirstOrDefault(g => g.MaNguoiDung == userIdString);

                if (gioHang == null || !gioHang.ChiTietGioHang.Any())
                {
                    TempData["Loi"] = "Giỏ hàng trống!";
                    return RedirectToAction("Index");
                }

                // Tính tổng tiền
                decimal tongTien = 0;
                foreach (var item in gioHang.ChiTietGioHang)
                {
                    var hangHoa = item.HangHoa;
                    decimal giaGoc = hangHoa.DonGia ?? 0;
                    decimal giamGia = hangHoa.GiamGia ?? 0;
                    decimal giaSauGiam = giaGoc * (1 - giamGia / 100);

                    decimal tienBH = 0;
                    if (item.BaoHanh1) tienBH += 990_000;
                    if (item.BaoHanh2) tienBH += 1_300_000;

                    decimal donGia = giaSauGiam + tienBH;
                    tongTien += donGia * item.SoLuong;
                }

                // Lấy phí giao hàng
                var phuongThucGiaoHang = _context.PhuongThucGiaoHang.Find(model.PhuongThucGiaoHangId);
                decimal phiGiaoHang = phuongThucGiaoHang?.PhiGiaoHang ?? 0;
                decimal tongTienSauPhiGiaoHang = tongTien + phiGiaoHang;

                // Tạo đơn hàng
                var donHang = new DonHang
                {
                    TaiKhoanId = userId,
                    TenDangNhap = User.Identity.Name,
                    NgayDat = DateTime.Now,
                    TongTien = tongTien,
                    TrangThai = "Chờ xác nhận",
                    PhuongThucThanhToanId = model.PhuongThucThanhToanId,
                    PhuongThucGiaoHangId = model.PhuongThucGiaoHangId,
                    PhiGiaoHang = phiGiaoHang,
                    TongTienSauPhiGiaoHang = tongTienSauPhiGiaoHang,
                    MaGiaoDich = GenerateTransactionId(),
                    TrangThaiThanhToan = "Đã thanh toán",
                    NgayThanhToan = DateTime.Now
                };

                _context.DonHang.Add(donHang);
                _context.SaveChanges();

                // Tạo thông tin giao hàng
                var thongTinGiaoHang = new ThongTinGiaoHang
                {
                    DonHangId = donHang.DonHangId,
                    HoTen = model.HoTen,
                    SoDienThoai = model.SoDienThoai,
                    DiaChi = model.DiaChi,
                    TinhThanh = model.TinhThanh,
                    QuanHuyen = model.QuanHuyen,
                    GhiChu = model.GhiChu
                };

                _context.ThongTinGiaoHang.Add(thongTinGiaoHang);

                // Tạo chi tiết đơn hàng
                foreach (var item in gioHang.ChiTietGioHang)
                {
                    var hangHoa = item.HangHoa;
                    decimal giaGoc = hangHoa.DonGia ?? 0;
                    decimal giamGia = hangHoa.GiamGia ?? 0;
                    decimal giaSauGiam = giaGoc * (1 - giamGia / 100);

                    decimal tienBH = 0;
                    if (item.BaoHanh1) tienBH += 990_000;
                    if (item.BaoHanh2) tienBH += 1_300_000;

                    decimal donGia = giaSauGiam + tienBH;
                    decimal thanhTien = donGia * item.SoLuong;

                    var chiTiet = new ChiTietDonHang
                    {
                        DonHangId = donHang.DonHangId,
                        MaHH = item.MaHH,
                        SoLuong = item.SoLuong,
                        DonGia = donGia,
                        ThanhTien = thanhTien,
                        BaoHanh1 = item.BaoHanh1,
                        BaoHanh2 = item.BaoHanh2
                    };

                    _context.ChiTietDonHang.Add(chiTiet);
                }

                // Xóa giỏ hàng
                _context.GioHangChiTietDb.RemoveRange(gioHang.ChiTietGioHang);
                _context.SaveChanges();

                TempData["ThanhToanThanhCong"] = "Thanh toán thẻ tín dụng thành công!";
                return RedirectToAction("ThanhCong", new { id = donHang.DonHangId });
            }
            catch (Exception ex)
            {
                TempData["Loi"] = $"Lỗi khi xử lý thanh toán: {ex.Message}";
                ViewBag.DanhSachNganHang = _context.NganHang.Where(n => n.HoatDong).OrderBy(n => n.ThuTu).ToList();
                return View("ThanhToanTheTinDung", model);
            }
        }

        // Trang xác nhận thanh toán
        public IActionResult XacNhanThanhToan(ThanhToanViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View(model);
        }

        private string GenerateTransactionId()
        {
            return $"TXN{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
        }

        // Helper method để load dropdown data
        private async Task LoadDropdownData(ThanhToanViewModel model)
        {
            model.DanhSachGiaoHang = await _context.PhuongThucGiaoHang
                .Where(p => p.HoatDong)
                .OrderBy(p => p.ThuTu)
                .ToListAsync();
            model.DanhSachThanhToan = await _context.PhuongThucThanhToan
                .Where(p => p.HoatDong)
                .OrderBy(p => p.ThuTu)
                .ToListAsync();
        }

        // Helper method để tính tổng tiền giỏ hàng
        private decimal TinhTongTienGioHang(GioHangDb gioHang)
        {
            decimal tongTien = 0;
            foreach (var item in gioHang.ChiTietGioHang)
            {
                if (item.HangHoa == null) continue;

                var hangHoa = item.HangHoa;
                decimal giaGoc = hangHoa.DonGia ?? 0;
                decimal giamGia = Math.Max(0, Math.Min(100, hangHoa.GiamGia ?? 0));
                decimal giaSauGiam = giaGoc * (1 - giamGia / 100);

                decimal tienBH = 0;
                if (item.BaoHanh1) tienBH += 990_000;
                if (item.BaoHanh2) tienBH += 1_300_000;

                decimal donGia = giaSauGiam + tienBH;
                tongTien += donGia * Math.Max(1, item.SoLuong);
            }
            return tongTien;
        }
    }
}