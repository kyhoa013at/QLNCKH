using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Client.Extensions;
using QLNCKH_HocVien.Client.Exceptions;

namespace QLNCKH_HocVien.Client.Services
{
    public class DashboardService
    {
        private readonly HttpClient _http;

        public DashboardService(HttpClient http)
        {
            _http = http;
        }

        /// <summary>
        /// Lấy danh sách hoạt động gần đây
        /// </summary>
        public async Task<List<RecentActivity>> GetRecentActivities(int limit = 10)
        {
            try
            {
                var result = await _http.GetFromJsonAsyncWithAuth<ApiResult<List<RecentActivity>>>(
                    $"api/Dashboard/recent-activities?limit={limit}");
                return result?.Data ?? new List<RecentActivity>();
            }
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (Exception)
            {
                return new List<RecentActivity>();
            }
        }
    }

    public class RecentActivity
    {
        public int Id { get; set; }
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string Icon { get; set; } = "";
        public string IconColor { get; set; } = "Default";
        public string TimeAgo { get; set; } = "";
    }
}

