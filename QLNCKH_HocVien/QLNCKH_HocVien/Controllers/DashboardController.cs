using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;
using QLNCKH_HocVien.Helpers;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Dashboard/recent-activities
        [HttpGet("recent-activities")]
        public async Task<ActionResult<ApiResult<List<RecentActivityDto>>>> GetRecentActivities([FromQuery] int limit = 10)
        {
            if (limit < 1) limit = 10;
            if (limit > 50) limit = 50;

            var activities = new List<RecentActivityDto>();

            // 1. Lấy các sản phẩm nộp gần đây (có NgayNop)
            var recentNopSanPham = await _context.NopSanPhams
                .OrderByDescending(nsp => nsp.NgayNop)
                .Take(limit)
                .ToListAsync();

            var chuyenDeDict = await _context.ChuyenDeNCKHs
                .Where(cd => recentNopSanPham.Select(n => n.IdChuyenDe).Contains(cd.Id))
                .ToDictionaryAsync(cd => cd.Id, cd => cd);

            foreach (var nsp in recentNopSanPham)
            {
                var cd = chuyenDeDict.GetValueOrDefault(nsp.IdChuyenDe);
                activities.Add(new RecentActivityDto
                {
                    Id = nsp.Id,
                    Type = "NopSanPham",
                    Title = $"Nộp sản phẩm: {cd?.MaSoCD ?? "N/A"}",
                    Description = $"Trạng thái: {GetTrangThaiText(nsp.TrangThai)}",
                    Timestamp = nsp.NgayNop,
                    Icon = "CloudUpload",
                    IconColor = "Info"
                });
            }

            // 2. Lấy các hội đồng chấm gần đây (có NgayCham)
            var recentHoiDong = await _context.HoiDongs
                .OrderByDescending(hd => hd.NgayCham)
                .Take(limit)
                .ToListAsync();

            var chuyenDeHdDict = await _context.ChuyenDeNCKHs
                .Where(cd => recentHoiDong.Select(hd => hd.IdChuyenDe).Contains(cd.Id))
                .ToDictionaryAsync(cd => cd.Id, cd => cd);

            foreach (var hd in recentHoiDong)
            {
                var cd = chuyenDeHdDict.GetValueOrDefault(hd.IdChuyenDe);
                activities.Add(new RecentActivityDto
                {
                    Id = hd.Id,
                    Type = "LapHoiDong",
                    Title = $"Lập hội đồng chấm: {cd?.MaSoCD ?? "N/A"}",
                    Description = $"Vòng: {GetVongThiText(hd.VongThi)}",
                    Timestamp = hd.NgayCham,
                    Icon = "Groups",
                    IconColor = "Tertiary"
                });
            }

            // 3. Lấy các phiếu chấm chung khảo gần đây (nếu có ngày từ HoiDong)
            // Lấy các hội đồng chung khảo đã có phiếu chấm
            var hoiDongChungKhao = await _context.HoiDongs
                .Where(hd => hd.VongThi == VongCham.ChungKhao)
                .OrderByDescending(hd => hd.NgayCham)
                .Take(limit)
                .ToListAsync();

            if (hoiDongChungKhao.Any())
            {
                var chuyenDeIds = hoiDongChungKhao.Select(hd => hd.IdChuyenDe).ToList();
                var hasPhieuCham = await _context.PhieuChams
                    .Where(pc => chuyenDeIds.Contains(pc.IdChuyenDe) && pc.Diem > 0)
                    .Select(pc => pc.IdChuyenDe)
                    .Distinct()
                    .ToListAsync();

                var chuyenDePcDict = await _context.ChuyenDeNCKHs
                    .Where(cd => hasPhieuCham.Contains(cd.Id))
                    .ToDictionaryAsync(cd => cd.Id, cd => cd);

                foreach (var hd in hoiDongChungKhao.Where(h => hasPhieuCham.Contains(h.IdChuyenDe)))
                {
                    var cd = chuyenDePcDict.GetValueOrDefault(hd.IdChuyenDe);
                    activities.Add(new RecentActivityDto
                    {
                        Id = hd.Id,
                        Type = "ChamDiem",
                        Title = $"Chấm điểm vòng chung khảo: {cd?.MaSoCD ?? "N/A"}",
                        Description = $"Ngày chấm: {hd.NgayCham:dd/MM/yyyy}",
                        Timestamp = hd.NgayCham,
                        Icon = "Grading",
                        IconColor = "Warning"
                    });
                }
            }

            // Sắp xếp theo thời gian và lấy top N
            var result = activities
                .OrderByDescending(a => a.Timestamp)
                .Take(limit)
                .ToList();

            return this.OkResult(result, $"Lấy {result.Count} hoạt động gần đây");
        }

        private string GetTrangThaiText(TrangThaiNop trangThai)
        {
            return trangThai switch
            {
                TrangThaiNop.NopBanMem => "Nộp bản mềm",
                TrangThaiNop.NopBanCung => "Nộp bản cứng",
                TrangThaiNop.NopSauChinhSua => "Nộp sau chỉnh sửa",
                _ => "Không xác định"
            };
        }

        private string GetVongThiText(VongCham vongThi)
        {
            return vongThi switch
            {
                VongCham.SoLoai => "Vòng Sơ Loại",
                VongCham.ChungKhao => "Vòng Chung Khảo",
                _ => "Không xác định"
            };
        }
    }

    public class RecentActivityDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string Icon { get; set; } = "";
        public string IconColor { get; set; } = "Default";
        public string TimeAgo => GetTimeAgo(Timestamp);

        private string GetTimeAgo(DateTime timestamp)
        {
            var now = DateTime.Now;
            var diff = now - timestamp;

            // Nếu thời gian trong tương lai (do lỗi dữ liệu), trả về "Vừa xong"
            if (diff.TotalSeconds < 0)
                return "Vừa xong";

            if (diff.TotalSeconds < 60)
                return "Vừa xong";
            
            if (diff.TotalMinutes < 60)
            {
                var minutes = (int)diff.TotalMinutes;
                return $"{minutes} phút trước";
            }
            
            if (diff.TotalHours < 24)
            {
                var hours = (int)diff.TotalHours;
                return $"{hours} giờ trước";
            }
            
            if (diff.TotalDays < 7)
            {
                var days = (int)diff.TotalDays;
                if (days == 1)
                    return "Hôm qua";
                return $"{days} ngày trước";
            }
            
            if (diff.TotalDays < 30)
            {
                var weeks = (int)(diff.TotalDays / 7);
                return $"{weeks} tuần trước";
            }
            
            if (diff.TotalDays < 365)
            {
                var months = (int)(diff.TotalDays / 30);
                return $"{months} tháng trước";
            }
            
            var years = (int)(diff.TotalDays / 365);
            return $"{years} năm trước";
        }
    }
}

