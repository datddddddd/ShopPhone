namespace ShopPhone.ViewModels
{
    public class ProductDetailViewModel
    {
        public int MaHH { get; set; }
        public string TenHH { get; set; }
        public string MoTaDonVi { get; set; }
        public decimal? DonGia { get; set; }
        public bool CoGiamGia => GiamGia.HasValue && GiamGia.Value > 0;

        public decimal GiaKhuyenMai => CoGiamGia
                    ? Math.Round(DonGia!.Value * (1 - GiamGia!.Value / 100), 0)
                    : DonGia ?? 0; public decimal? GiamGia { get; set; }

        public string Hinh { get; set; }
        public DateTime NgaySX { get; set; }
        public int SoLanXem { get; set; }
        public string MoTa { get; set; }
        public float DanhGia { get; set; }
        public string HinhMoHop { get; set; }
        public string HinhThucTe { get; set; }
        public string VideoId { get; set; }
        public string YouTubeUrl => $"https://www.youtube.com/embed/{VideoId}";
    }
}