using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPhone.Migrations
{
    /// <inheritdoc />
    public partial class PaymentAndDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "NganHang",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TenNganHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        MaNganHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
            //        Logo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
            //        LoaiThe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        MauThe = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        HoatDong = table.Column<bool>(type: "bit", nullable: false),
            //        ThuTu = table.Column<int>(type: "int", nullable: false),
            //        NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_NganHang", x => x.Id);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "NganHang");
        }
    }
}
