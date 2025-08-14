using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopPhone.Models;
using ShopPhone.Services;
using System.Security.Claims;

namespace ShopPhone.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<TaiKhoan> _hasher;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/account/Login.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login model, string? returnUrl)
        {
            if (!ModelState.IsValid)
                return View("~/Views/account/Login.cshtml");

            var user = await _context.TaiKhoan
                .SingleOrDefaultAsync(x => x.Email == model.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Email chưa được đăng ký.");
                return View("~/Views/account/Login.cshtml", model);
            }

            var result = _hasher.VerifyHashedPassword(null!, user.MatKhau, model.Password);
            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Sai mật khẩu.");
                return View("~/Views/account/Login.cshtml", model);
            }

            var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.Email),
    new Claim(ClaimTypes.Role, user.VaiTro),
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // <- thêm dòng này
};

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            TempData["ThongBao"] = $"Đăng nhập thành công với vai trò {(user.VaiTro == "Admin" ? "quản trị viên" : "người dùng")}.";

            // ✅ Sử dụng returnUrl nếu có
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Có thể thêm TempData nếu cần thông báo
            TempData["ThongBao"] = "Bạn đã đăng xuất thành công.";

            return RedirectToAction("Login", "Account"); // quay về trang login
        }

        //Đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra email tồn tại
            var emailDaTonTai = _context.TaiKhoan.Any(x => x.Email == model.Email);
            if (emailDaTonTai)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại");
                return View(model);
            }

            var tk = new TaiKhoan
            {
                HoTen = model.HoTen,
                Email = model.Email,
                MatKhau = _hasher.HashPassword(null!, model.Password),
                VaiTro = "User",
                NgayTao = DateTime.Now
            };

            _context.TaiKhoan.Add(tk);
            await _context.SaveChangesAsync();

            TempData["ThongBao"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay";
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> ChiTiet(int id)          // id = MaHH
        {
            var sp = await _context.HangHoa
                                   .FirstOrDefaultAsync(h => h.MaHH == id);

            if (sp == null)
                return NotFound();      // 404 nếu không có

            return View(sp);            // truyền model sang view
        }

        // gửi otp về gmail
        private readonly EmailSender _emailSender;

        public AccountController(ApplicationDbContext context, IPasswordHasher<TaiKhoan> hasher, EmailSender emailSender)
        {
            _context = context;
            _hasher = hasher;
            _emailSender = emailSender;
        }

        // Giao diện nhập email
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Console.WriteLine("EMAIL NHẬN ĐƯỢC: " + model.Email); // Kiểm tra đầu vào

            string otp = new Random().Next(100000, 999999).ToString();
            TempData["OTP"] = otp;
            TempData["Email"] = model.Email;

            string body = $"Mã OTP đặt lại mật khẩu của bạn là: <b>{otp}</b>";

            try
            {
                await _emailSender.SendEmailAsync(model.Email, "Mã OTP lấy lại mật khẩu", body);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Gửi email thất bại: " + ex.Message);
                return View(model);
            }

            return RedirectToAction("VerifyOTP");
        }

        public IActionResult VerifyOTP() => View();

        [HttpPost]
        public IActionResult VerifyOTP(VerifyOTPViewModel model)
        {
            string savedOtp = TempData["OTP"] as string;
            string email = TempData["Email"] as string;

            if (model.OTP == savedOtp)
            {
                TempData["Email"] = email;
                return RedirectToAction("ResetPassword");
            }

            ModelState.AddModelError("", "Mã OTP không đúng");
            return View(model);
        }

        public IActionResult ResetPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string password)
        {
            string email = TempData["Email"] as string;

            // TODO: Tìm user theo email, cập nhật mật khẩu mới (nên hash)

            TempData["Success"] = "Mật khẩu đã được cập nhật!";
            return RedirectToAction("Login", "Account");
        }
    }
}