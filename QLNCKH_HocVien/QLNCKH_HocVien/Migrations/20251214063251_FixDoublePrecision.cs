using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNCKH_HocVien.Migrations
{
    /// <inheritdoc />
    public partial class FixDoublePrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "DiemTrungBinh",
                table: "XepGiais",
                type: "float(53)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(4)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Diem",
                table: "PhieuChams",
                type: "float(53)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(4)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "DiemSo",
                table: "KetQuaSoLoais",
                type: "float(53)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(4)",
                oldPrecision: 4,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "DiemTrungBinh",
                table: "XepGiais",
                type: "float(4)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(53)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Diem",
                table: "PhieuChams",
                type: "float(4)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(53)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "DiemSo",
                table: "KetQuaSoLoais",
                type: "float(4)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(53)",
                oldPrecision: 4,
                oldScale: 2);
        }
    }
}
