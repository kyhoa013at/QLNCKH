using QLNCKH_HocVien.Client.Extensions;
using QLNCKH_HocVien.Client.Models;
using System.Net.Http.Json;

namespace QLNCKH_HocVien.Client.Services
{
    /// <summary>
    /// Service quản lý người dùng (Admin only)
    /// </summary>
    public class UserManagementService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserManagementService> _logger;

        public UserManagementService(HttpClient httpClient, ILogger<UserManagementService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách tất cả người dùng (có pagination)
        /// </summary>
        public async Task<PaginatedResult<UserDto>?> GetAllUsersAsync(
            int page = 1,
            int pageSize = 10,
            string? search = null,
            string? role = null,
            bool? isActive = null)
        {
            try
            {
                var queryParams = new List<string>
                {
                    $"page={page}",
                    $"pageSize={pageSize}"
                };

                if (!string.IsNullOrWhiteSpace(search))
                    queryParams.Add($"search={Uri.EscapeDataString(search)}");

                if (!string.IsNullOrWhiteSpace(role))
                    queryParams.Add($"role={Uri.EscapeDataString(role)}");

                if (isActive.HasValue)
                    queryParams.Add($"isActive={isActive.Value}");

                var url = $"/api/usermanagement?{string.Join("&", queryParams)}";
                return await _httpClient.GetFromJsonAsyncWithAuth<PaginatedResult<UserDto>>(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách người dùng");
                return null;
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết một người dùng
        /// </summary>
        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsyncWithAuth<ApiResult<UserDto>>($"/api/usermanagement/{id}");
                return result?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin người dùng {UserId}", id);
                return null;
            }
        }

        /// <summary>
        /// Cập nhật role của người dùng
        /// </summary>
        public async Task<ApiResult?> UpdateUserRoleAsync(int id, string vaiTro)
        {
            try
            {
                var request = new { VaiTro = vaiTro };
                var response = await _httpClient.PutAsJsonAsyncWithAuth($"/api/usermanagement/{id}/role", request);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResult>();
                }
                else
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<ApiResult>();
                    return errorResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật role của user {UserId}", id);
                return new ApiResult { Success = false, Message = "Lỗi khi cập nhật quyền" };
            }
        }

        /// <summary>
        /// Cập nhật trạng thái kích hoạt của người dùng
        /// </summary>
        public async Task<ApiResult?> UpdateUserStatusAsync(int id, bool isActive)
        {
            try
            {
                var request = new { IsActive = isActive };
                var response = await _httpClient.PutAsJsonAsyncWithAuth($"/api/usermanagement/{id}/status", request);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResult>();
                }
                else
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<ApiResult>();
                    return errorResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái user {UserId}", id);
                return new ApiResult { Success = false, Message = "Lỗi khi cập nhật trạng thái" };
            }
        }

        /// <summary>
        /// Xóa người dùng
        /// </summary>
        public async Task<ApiResult?> DeleteUserAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsyncWithAuth($"/api/usermanagement/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResult>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var errorResult = await response.Content.ReadFromJsonAsync<ApiResult>();
                        return errorResult;
                    }
                    catch
                    {
                        return new ApiResult { Success = false, Message = errorContent };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa user {UserId}", id);
                return new ApiResult { Success = false, Message = "Lỗi khi xóa tài khoản" };
            }
        }

        /// <summary>
        /// Lấy thống kê người dùng
        /// </summary>
        public async Task<UserStatsDto?> GetUserStatsAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsyncWithAuth<ApiResult<UserStatsDto>>("/api/usermanagement/stats");
                return result?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thống kê người dùng");
                return null;
            }
        }
    }

    /// <summary>
    /// DTO cho thông tin người dùng
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string TenDangNhap { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string VaiTro { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime NgayTao { get; set; }
    }

    /// <summary>
    /// DTO cho thống kê người dùng
    /// </summary>
    public class UserStatsDto
    {
        public int Total { get; set; }
        public int Active { get; set; }
        public int Inactive { get; set; }
        public int Admin { get; set; }
        public int GiaoVien { get; set; }
        public int User { get; set; }
    }
}

