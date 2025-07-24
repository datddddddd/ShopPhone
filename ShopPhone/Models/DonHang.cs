using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPhone.Models // ⚠ Đảm bảo giống với project thật của bạn
{
    public class DonHang
    {
        public bool DaXacNhan { get; set; } = false;

        public int DonHangId { get; set; }

        public int TaiKhoanId { get; set; }  // nếu bạn đổi sang kiểu khoá ngoại mới

        public string? TenDangNhap { get; set; } // <== bạn cần dòng này nếu đang dùng theo tên đăng nhập

        public DateTime NgayDat { get; set; }
        

        public decimal TongTien { get; set; }

        public string TrangThai { get; set; }

        public List<ChiTietDonHang> ChiTietDonHang { get; set; } = new();
    }


}
