using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopPhone.Models;
using ShopPhone.Services;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ DbContext với connection string từ appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopPhoneConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<EmailSender>();

// 🔧 Thêm dòng này để kích hoạt dịch vụ Session
builder.Services.AddSession();

builder.Services.AddScoped<IPasswordHasher<TaiKhoan>, PasswordHasher<TaiKhoan>>();

// Thiết lập quên mật khẩu gửi OTP vê
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Phân quyền truy cập 
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// 🔧 Thêm dòng này để bật middleware Session
app.UseSession();

app.UseRouting();

app.UseAuthentication(); // ⬅️ bắt buộc nếu bạn dùng Claims
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
