using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPhone.Models
{
    public class GioHangChiTietDb
    {
        [Key]
        public int Id { get; set; }

        // FK -> GioHangDb
        public int GioHangDbId { get; set; }

        // FK -> HangHoa
        public int MaHH { get; set; }

        public bool BaoHanh1 { get; set; } = false;
        public bool BaoHanh2 { get; set; } = false;

        public int SoLuong { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DonGia { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? GiamGia { get; set; }

        public int GoiBaoHanh { get; set; }

        /* ------------ Navigation ------------ */

        [ForeignKey(nameof(GioHangDbId))]
        public GioHangDb? GioHang { get; set; }

        [ForeignKey(nameof(MaHH))]
        public HangHoa? HangHoa { get; set; }
    }
}