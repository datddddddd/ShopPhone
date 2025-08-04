using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopPhone.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ShopPhone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        // Inject logger và DbContext
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Trang chủ: Lọc + phân trang sản phẩm
        public IActionResult Index(decimal? giaTu, decimal? giaDen, string tuKhoa, int trang = 1)
        {
            int pageSize = 8;

            // B1: Truy vấn dữ liệu gốc (chỉ lấy từ DB những cột có thật)
            var productQuery = _context.HangHoa
                .Where(p =>
                    (!giaTu.HasValue || (p.DonGia ?? 0) >= giaTu.Value) &&
                    (!giaDen.HasValue || (p.DonGia ?? 0) <= giaDen.Value) &&
                    (string.IsNullOrEmpty(tuKhoa) || p.TenHH.Contains(tuKhoa)));

            int totalProducts = productQuery.Count();

            // B2: Thực hiện ToList() trước khi xử lý giá tính toán
            var productRawList = productQuery
                .OrderBy(p => p.TenHH)
                .Skip((trang - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // B3: Dựng ViewModel và tính giá
            var result = productRawList.Select(p => new HangHoa
            {
                MaHH = p.MaHH,
                TenHH = p.TenHH,
                Hinh = p.Hinh,
                DonGia = p.DonGia,
                GiamGia = p.GiamGia,
                SoLanXem = p.SoLanXem,
                NgaySX = p.NgaySX,
            }).ToList();

            // LẤY SẢN PHẨM NỔI BẬT (ví dụ: GiamGia > 0 hoặc SoLanXem cao)
            var sanPhamNoiBat = _context.HangHoa
                .Where(p => p.GiamGia > 0) // hoặc điều kiện khác tuỳ bạn
                .OrderByDescending(p => p.SoLanXem)
                .Take(5)
                .ToList();

            var vm = new ProductFilterViewModel
            {
                DanhSachSanPham = result,
                GiaTu = giaTu,
                GiaDen = giaDen,
                TuKhoa = tuKhoa,
                TrangHienTai = trang,
                SoTrang = (int)Math.Ceiling((double)totalProducts / pageSize),
                TongSoSanPham = totalProducts,
                SanPhamNoiBat = sanPhamNoiBat
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult QuanLyDonHang()
        {
            var dsDon = _context.DonHang
                .Include(d => d.ChiTietDonHang)
                .ThenInclude(c => c.HangHoa)
                .ToList();

            return View(dsDon);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DanhSachLienHe()
        {
            var danhSachLienHe = _context.LienHe
                .OrderByDescending(lh => lh.NgayGui)
                .ToList();

            return View(danhSachLienHe);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Cho phép truy cập ẩn danh nếu toàn bộ controller đang [Authorize]

        public IActionResult GioiThieu()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult LienHe()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult LienHe(LienHe model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ThongBao"] = "Bạn cần đăng nhập để gửi liên hệ!";
                return RedirectToAction("LienHe");
            }

            if (ModelState.IsValid)
            {
                model.NgayGui = DateTime.Now;
                model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.LienHe.Add(model);
                _context.SaveChanges();

                TempData["ThongBao"] = "Gửi liên hệ thành công!";
                return RedirectToAction("LienHe");
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var hangHoa = _context.HangHoa.FirstOrDefault(h => h.MaHH == id);
            if (hangHoa == null)
                return NotFound();

            ViewBag.MaNCCList = GetMaNCCList();
            ViewBag.DsMaLoai = GetDsMaLoai();

            return View(hangHoa);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HangHoa hangHoa, IFormFile? Hinh)
        {
            if (id != hangHoa.MaHH)
                return NotFound();

            if (ModelState.IsValid)
            {
                var sp = await _context.HangHoa.FindAsync(id);
                if (sp == null)
                    return NotFound();

                // Cập nhật thông tin cơ bản
                sp.TenHH = hangHoa.TenHH;
                sp.TenAlias = hangHoa.TenAlias;
                sp.MoTa = hangHoa.MoTa;
                sp.DonGia = hangHoa.DonGia;
                sp.MaLoai = hangHoa.MaLoai;
                sp.MaNCC = hangHoa.MaNCC;
                sp.NgaySX = hangHoa.NgaySX;
                sp.GiamGia = hangHoa.GiamGia;
                sp.VideoId = hangHoa.VideoId;
                sp.MoTaDonVi = hangHoa.MoTaDonVi;

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(path); // đảm bảo thư mục tồn tại

                // Xử lý ảnh chính
                if (Hinh != null && Hinh.Length > 0)
                {
                    var fileName = Path.GetFileName(Hinh.FileName);
                    var fullPath = Path.Combine(path, fileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await Hinh.CopyToAsync(stream);
                    sp.Hinh = fileName;
                }

                // Ảnh mở hộp
                if (hangHoa.FileHinhMoHop != null && hangHoa.FileHinhMoHop.Length > 0)
                {
                    var fileName = Path.GetFileName(hangHoa.FileHinhMoHop.FileName);
                    var fullPath = Path.Combine(path, fileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await hangHoa.FileHinhMoHop.CopyToAsync(stream);
                    sp.HinhMoHop = fileName;
                }
                else
                {
                    sp.HinhMoHop = hangHoa.HinhMoHop; // giữ ảnh cũ
                }
                // Ảnh thực tế
                if (hangHoa.FileHinhThucTe != null && hangHoa.FileHinhThucTe.Length > 0)
                {
                    var fileName = Path.GetFileName(hangHoa.FileHinhThucTe.FileName);
                    var fullPath = Path.Combine(path, fileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await hangHoa.FileHinhThucTe.CopyToAsync(stream);
                    sp.HinhThucTe = fileName;
                }
                else
                {
                    sp.HinhThucTe = hangHoa.HinhThucTe; // giữ ảnh cũ
                }

                // Lưu vào database
                _context.Update(sp);
                await _context.SaveChangesAsync();

                // Thông báo thành công
                TempData["SuccessMessage"] = "✅ Sửa sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            // Nếu lỗi thì load lại danh sách dropdown
            ViewBag.MaNCCList = GetMaNCCList();
            ViewBag.DsMaLoai = GetDsMaLoai();
            return View(hangHoa);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Account");

            var user = _context.TaiKhoan.FirstOrDefault(tk => tk.Email == email);
            if (user == null)
                return NotFound();

            return View(user); // truyền model TaiKhoan sang View
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(TaiKhoan model)
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Account");

            var user = _context.TaiKhoan.FirstOrDefault(tk => tk.Email == email);
            if (user == null) return NotFound();

            // ❌ Không cho admin sửa
            if (user.VaiTro == "Admin")
            {
                TempData["Loi"] = "Admin không được phép sửa thông tin.";
                return RedirectToAction("Profile");
            }

            // ✅ Cập nhật họ tên
            user.HoTen = model.HoTen;

            // ✅ Xử lý ảnh đại diện
            if (model.FileAnhDaiDien != null && model.FileAnhDaiDien.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");
                Directory.CreateDirectory(uploadsFolder); // tạo thư mục nếu chưa có

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.FileAnhDaiDien.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FileAnhDaiDien.CopyToAsync(stream);
                }

                user.AnhDaiDien = fileName;
            }
            else
            {
                // Nếu không chọn ảnh mới, giữ lại ảnh cũ
                user.AnhDaiDien = model.AnhDaiDien;
            }

            // ✅ Lưu
            _context.SaveChanges();

            TempData["ThongBao"] = "✅ Cập nhật thông tin thành công!";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var sp = _context.HangHoa.Find(id);
            if (sp == null)
            {
                return NotFound();
            }

            _context.HangHoa.Remove(sp);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Đã xóa sản phẩm thành công!";

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()

        {
            ViewBag.MaNCCList = new List<SelectListItem>
{
    new SelectListItem { Value = "AP", Text = "AP" },
    new SelectListItem { Value = "MO", Text = "MO" },
    new SelectListItem { Value = "NK", Text = "NK" },
    new SelectListItem { Value = "OP", Text = "OP" },
    new SelectListItem { Value = "PK", Text = "PK" },
    new SelectListItem { Value = "SM", Text = "SM" },
    new SelectListItem { Value = "SS", Text = "SS" },
    new SelectListItem { Value = "XI", Text = "XI" }
};

            ViewBag.DsMaLoai = new List<SelectListItem>
    {
        new SelectListItem { Value = "1000", Text = "1000 - Iphone" },
        new SelectListItem { Value = "1001", Text = "1001 - SamSung" },
        new SelectListItem { Value = "1002", Text = "1002 - Oppo" },
        new SelectListItem { Value = "1003", Text = "1003 - Xiaomi" },
        new SelectListItem { Value = "1004", Text = "1004 - Vivo" },
        new SelectListItem { Value = "1005", Text = "1005 - Realme" },
        new SelectListItem { Value = "1006", Text = "1006 - Honor" },
        new SelectListItem { Value = "1007", Text = "1007 - Laptop" },
        new SelectListItem { Value = "1008", Text = "1008 - Phụ Kiện" }
    };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(HangHoa hangHoa, IFormFile FileHinh, IFormFile FileHinhMoHop, IFormFile FileHinhThucTe)
        {
            hangHoa.FileHinh = FileHinh;
            hangHoa.FileHinhMoHop = FileHinhMoHop;
            hangHoa.FileHinhThucTe = FileHinhThucTe;

            if (ModelState.IsValid)
            {
                // Tạo thư mục wwwroot/images nếu chưa có
                var pathSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(pathSave))
                    Directory.CreateDirectory(pathSave);

                // Lưu hình chính
                if (FileHinh != null && FileHinh.Length > 0)
                {
                    var fileName = Path.GetFileName(FileHinh.FileName);
                    var fullPath = Path.Combine(pathSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await FileHinh.CopyToAsync(stream);
                    }
                    hangHoa.Hinh = fileName;
                }

                // Lưu hình mở hộp
                if (FileHinhMoHop != null && FileHinhMoHop.Length > 0)
                {
                    var fileName = Path.GetFileName(FileHinhMoHop.FileName);
                    var fullPath = Path.Combine(pathSave, fileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await FileHinhMoHop.CopyToAsync(stream);
                    hangHoa.HinhMoHop = fileName;
                }

                // Lưu hình thực tế
                if (FileHinhThucTe != null && FileHinhThucTe.Length > 0)
                {
                    var fileName = Path.GetFileName(FileHinhThucTe.FileName);
                    var fullPath = Path.Combine(pathSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await FileHinhThucTe.CopyToAsync(stream);
                    }
                    hangHoa.HinhThucTe = fileName;
                }
                hangHoa.SoLanXem = 0;
                _context.Add(hangHoa);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";

                return RedirectToAction(nameof(Index));
            }
            ViewBag.MaNCCList = new List<SelectListItem>
{
    new SelectListItem { Value = "AP", Text = "AP" },
    new SelectListItem { Value = "MO", Text = "MO" },
    new SelectListItem { Value = "NK", Text = "NK" },
    new SelectListItem { Value = "OP", Text = "OP" },
    new SelectListItem { Value = "PK", Text = "PK" },
    new SelectListItem { Value = "SM", Text = "SM" },
    new SelectListItem { Value = "SS", Text = "SS" },
    new SelectListItem { Value = "XI", Text = "XI" }
};

            ViewBag.DsMaLoai = new List<SelectListItem>
{
        new SelectListItem { Value = "1000", Text = "1000 - Iphone" },
        new SelectListItem { Value = "1001", Text = "1001 - SamSung" },
        new SelectListItem { Value = "1002", Text = "1002 - Oppo" },
        new SelectListItem { Value = "1003", Text = "1003 - Xiaomi" },
        new SelectListItem { Value = "1004", Text = "1004 - Vivo" },
        new SelectListItem { Value = "1005", Text = "1005 - Realme" },
        new SelectListItem { Value = "1006", Text = "1006 - Honor" },
        new SelectListItem { Value = "1007", Text = "1007 - Laptop" },
        new SelectListItem { Value = "1008", Text = "1008 - Phụ Kiện" }
};
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    foreach (var error in entry.Value.Errors)
                    {
                        _logger.LogWarning("❌ ModelState Error: Field '{Field}' - {Error}", key, error.ErrorMessage);
                    }
                }
            }

            return View(hangHoa);
        }

        // Hàm lấy danh sách nhà cung cấp
        private List<SelectListItem> GetMaNCCList()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Value = "AP", Text = "AP" },
        new SelectListItem { Value = "MO", Text = "MO" },
        new SelectListItem { Value = "NK", Text = "NK" },
        new SelectListItem { Value = "OP", Text = "OP" },
        new SelectListItem { Value = "PK", Text = "PK" },
        new SelectListItem { Value = "SM", Text = "SM" },
        new SelectListItem { Value = "SS", Text = "SS" },
        new SelectListItem { Value = "XI", Text = "XI" }
    };
        }

        // Hàm lấy danh sách mã loại
        private List<SelectListItem> GetDsMaLoai()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Value = "1000", Text = "1000 - Iphone" },
        new SelectListItem { Value = "1001", Text = "1001 - Samsung" },
        new SelectListItem { Value = "1002", Text = "1002 - Oppo" },
        new SelectListItem { Value = "1003", Text = "1003 - Xiaomi" },
        new SelectListItem { Value = "1004", Text = "1004 - Vivo" },
        new SelectListItem { Value = "1005", Text = "1005 - Realme" },
        new SelectListItem { Value = "1006", Text = "1006 - Honor" },
        new SelectListItem { Value = "1007", Text = "1007 - Laptop" },
        new SelectListItem { Value = "1008", Text = "1008 - Phụ Kiện" }
    };
        }
    }
}