namespace ShopPhone.Models
{
    public class ProductFilterViewModel
    {
        public List<HangHoa> DanhSachSanPham { get; set; } = new List<HangHoa>();
        public int SoTrang { get; set; }
        public int TrangHienTai { get; set; }

        public decimal? GiaTu { get; set; }
        public decimal? GiaDen { get; set; }
        public string TuKhoa { get; set; }
        public List<HangHoa> SanPhamNoiBat { get; set; }

        public int TongSoSanPham { get; set; }
    }
}