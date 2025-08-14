namespace ShopPhone.Models
{
    public class CapNhatBaoHanh
    {
        public int Id { get; set; }           // ID của dòng giỏ hàng
        public bool BaoHanh1 { get; set; }    // Gói bảo hành thường
        public bool BaoHanh2 { get; set; }    // Gói premium
        public int SoLuong { get; set; }
    }
}