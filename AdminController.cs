using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopPhone.Models;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult Orders()
    {
        var donHang = _context.DonHang
            .Include(d => d.ChiTietDonHang)
                .ThenInclude(ct => ct.HangHoa)
            .ToList();

        return View(donHang);
    }

    public IActionResult Users()
    {
        return View();
    }

    public IActionResult Products()
    {
        return View();
    }
    [HttpPost]
    public IActionResult XacNhanDonHang(int id)
    {
        var donHang = _context.DonHang.FirstOrDefault(d => d.DonHangId == id);
        if (donHang == null)
        {
            TempData["Loi"] = "Không tìm thấy đơn hàng!";
            return RedirectToAction("DanhSachDonHang");
        }

        // Cập nhật trạng thái đơn hàng
        donHang.TrangThai = "Đã xác nhận";
        donHang.DaXacNhan = true;
        _context.SaveChanges();

        TempData["ThongBao"] = "✅ Xác nhận đơn hàng thành công!";
        return RedirectToAction("Orders", "Admin");
    }

    // GET: /Admin/Feedbacks
    public async Task<IActionResult> Feedbacks()
    {
        var feedbacks = await _context.LienHe.OrderByDescending(f => f.NgayGui).ToListAsync();
        return View(feedbacks);
    }
    public IActionResult Statistics()
    {
        // Lọc các đơn hàng đã xác nhận (DaXacNhan == true)
        var donHangDaXacNhan = _context.DonHang
            .Where(d => d.DaXacNhan == true)
            .ToList();

        // Tính tổng số đơn và tổng doanh thu
        ViewBag.TongDon = donHangDaXacNhan.Count;
        ViewBag.TongDoanhThu = donHangDaXacNhan.Sum(d => d.TongTien);

        // Nhóm theo tháng
        var doanhThuTheoThang = donHangDaXacNhan
            .GroupBy(d => d.NgayDat.ToString("MM/yyyy"))
            .OrderBy(g => g.Key)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(d => d.TongTien)
            );

        ViewBag.ChartLabels = doanhThuTheoThang.Keys.ToList();   // danh sách các tháng
        ViewBag.ChartValues = doanhThuTheoThang.Values.ToList(); // tổng doanh thu theo tháng

        return View(donHangDaXacNhan);
    }

}
