using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPhone.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingDonHangColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Kiểm tra và thêm cột PhuongThucThanhToanId nếu chưa có
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'PhuongThucThanhToanId')
                BEGIN
                    ALTER TABLE [DonHang] ADD [PhuongThucThanhToanId] int NULL
                END
            ");

            // Kiểm tra và thêm cột MaGiaoDich nếu chưa có
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'MaGiaoDich')
                BEGIN
                    ALTER TABLE [DonHang] ADD [MaGiaoDich] nvarchar(max) NULL
                END
            ");

            // Kiểm tra và thêm cột TrangThaiThanhToan nếu chưa có
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'TrangThaiThanhToan')
                BEGIN
                    ALTER TABLE [DonHang] ADD [TrangThaiThanhToan] nvarchar(max) NULL DEFAULT 'Chưa thanh toán'
                END
            ");

            // Kiểm tra và thêm cột NgayThanhToan nếu chưa có
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'NgayThanhToan')
                BEGIN
                    ALTER TABLE [DonHang] ADD [NgayThanhToan] datetime2 NULL
                END
            ");

            // Kiểm tra và thêm cột PhuongThucGiaoHangId nếu chưa có
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'PhuongThucGiaoHangId')
                BEGIN
                    ALTER TABLE [DonHang] ADD [PhuongThucGiaoHangId] int NULL
                END
            ");

            // Kiểm tra và thêm cột PhiGiaoHang nếu chưa có
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'PhiGiaoHang')
                BEGIN
                    ALTER TABLE [DonHang] ADD [PhiGiaoHang] decimal(18,2) NOT NULL DEFAULT 0
                END
            ");

            // Kiểm tra và thêm cột TongTienSauPhiGiaoHang nếu chưa có
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'TongTienSauPhiGiaoHang')
                BEGIN
                    ALTER TABLE [DonHang] ADD [TongTienSauPhiGiaoHang] decimal(18,2) NOT NULL DEFAULT 0
                END
            ");

            // Thêm foreign key constraints nếu chưa có
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_DonHang_PhuongThucThanhToan_PhuongThucThanhToanId')
                BEGIN
                    ALTER TABLE [DonHang] ADD CONSTRAINT [FK_DonHang_PhuongThucThanhToan_PhuongThucThanhToanId] 
                    FOREIGN KEY ([PhuongThucThanhToanId]) REFERENCES [PhuongThucThanhToan] ([Id])
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_DonHang_PhuongThucGiaoHang_PhuongThucGiaoHangId')
                BEGIN
                    ALTER TABLE [DonHang] ADD CONSTRAINT [FK_DonHang_PhuongThucGiaoHang_PhuongThucGiaoHangId] 
                    FOREIGN KEY ([PhuongThucGiaoHangId]) REFERENCES [PhuongThucGiaoHang] ([Id])
                END
            ");

            // Tạo indexes nếu chưa có
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_DonHang_PhuongThucThanhToanId')
                BEGIN
                    CREATE INDEX [IX_DonHang_PhuongThucThanhToanId] ON [DonHang] ([PhuongThucThanhToanId])
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_DonHang_PhuongThucGiaoHangId')
                BEGIN
                    CREATE INDEX [IX_DonHang_PhuongThucGiaoHangId] ON [DonHang] ([PhuongThucGiaoHangId])
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_DonHang_PhuongThucThanhToan_PhuongThucThanhToanId')
                BEGIN
                    ALTER TABLE [DonHang] DROP CONSTRAINT [FK_DonHang_PhuongThucThanhToan_PhuongThucThanhToanId]
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_DonHang_PhuongThucGiaoHang_PhuongThucGiaoHangId')
                BEGIN
                    ALTER TABLE [DonHang] DROP CONSTRAINT [FK_DonHang_PhuongThucGiaoHang_PhuongThucGiaoHangId]
                END
            ");

            // Drop indexes
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_DonHang_PhuongThucThanhToanId')
                BEGIN
                    DROP INDEX [IX_DonHang_PhuongThucThanhToanId] ON [DonHang]
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_DonHang_PhuongThucGiaoHangId')
                BEGIN
                    DROP INDEX [IX_DonHang_PhuongThucGiaoHangId] ON [DonHang]
                END
            ");

            // Drop columns
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'PhuongThucThanhToanId')
                BEGIN
                    ALTER TABLE [DonHang] DROP COLUMN [PhuongThucThanhToanId]
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'MaGiaoDich')
                BEGIN
                    ALTER TABLE [DonHang] DROP COLUMN [MaGiaoDich]
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'TrangThaiThanhToan')
                BEGIN
                    ALTER TABLE [DonHang] DROP COLUMN [TrangThaiThanhToan]
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'NgayThanhToan')
                BEGIN
                    ALTER TABLE [DonHang] DROP COLUMN [NgayThanhToan]
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'PhuongThucGiaoHangId')
                BEGIN
                    ALTER TABLE [DonHang] DROP COLUMN [PhuongThucGiaoHangId]
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'PhiGiaoHang')
                BEGIN
                    ALTER TABLE [DonHang] DROP COLUMN [PhiGiaoHang]
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DonHang]') AND name = 'TongTienSauPhiGiaoHang')
                BEGIN
                    ALTER TABLE [DonHang] DROP COLUMN [TongTienSauPhiGiaoHang]
                END
            ");
        }
    }
}
