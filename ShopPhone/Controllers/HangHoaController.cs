// Đường dẫn: Controllers/SanPhamController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopPhone.Models;
using ShopPhone.ViewModels;

[Authorize]
public class HangHoaController : Controller
{
    private readonly ApplicationDbContext _context;

    public HangHoaController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string? tuKhoa, int? giaTu, int? giaDen)
    {
        var query = _context.HangHoa.AsQueryable();

        if (!string.IsNullOrEmpty(tuKhoa))
            query = query.Where(sp => sp.TenHH.Contains(tuKhoa));

        if (giaTu.HasValue)
            query = query.Where(sp => sp.DonGia >= giaTu);

        if (giaDen.HasValue)
            query = query.Where(sp => sp.DonGia <= giaDen);

        var model = new ProductFilterViewModel
        {
            TuKhoa = tuKhoa,
            GiaTu = giaTu,
            GiaDen = giaDen,
            DanhSachSanPham = query.ToList()
        };

        return View(model);
    }

    public IActionResult ChiTiet(int id)
    {
        var hanghoa = _context.HangHoa.FirstOrDefault(h => h.MaHH == id);
        if (hanghoa == null) return NotFound();

        hanghoa.SoLanXem = hanghoa.SoLanXem + 1;
        _context.SaveChanges();
        var vm = new ProductDetailViewModel
        {
            MaHH = hanghoa.MaHH,
            TenHH = hanghoa.TenHH,
            MoTaDonVi = hanghoa.MoTaDonVi,
            DonGia = hanghoa.DonGia ?? 0,
            GiamGia = hanghoa.GiamGia ?? 0,
            Hinh = hanghoa.Hinh,
            NgaySX = hanghoa.NgaySX,
            SoLanXem = hanghoa.SoLanXem ?? 0,
            MoTa = hanghoa.MoTa,
            DanhGia = (float)(hanghoa.DanhGia ?? 0.0),
            HinhMoHop = hanghoa.HinhMoHop,
            HinhThucTe = hanghoa.HinhThucTe,
            VideoId = hanghoa.VideoId
        };

        return View(vm); // Tự tìm Views/SanPham/ChiTiet.cshtml
    }

    public IActionResult DanhSachTheoLoai(int maloai)
    {
        // Xử lý lọc sản phẩm theo mã loại
        var ds = _context.HangHoa.Where(h => h.MaLoai == maloai).ToList();
        return View(ds);
    }

    [HttpGet]
    public IActionResult SearchApi(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Json(new List<object>());

        var kq = _context.HangHoa
            .Where(h => h.TenHH.Contains(query))
            .Select(h => new
            {
                h.MaHH,
                h.TenHH,
                h.Hinh,
                DonGia = h.DonGia ?? 0
            })
            .ToList();

        return Json(kq);
    }

    // GET: HangHoa/Edit/5
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var sp = _context.HangHoa.Find(id);
        if (sp == null) return NotFound();
        return View(sp);
    }

    // GET: HangHoa/Delete/5
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

        // Chuyển hướng về HomeController, action Index
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(HangHoa hh)
    {
        if (ModelState.IsValid)
        {
            _context.HangHoa.Add(hh);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(hh);
    }
}