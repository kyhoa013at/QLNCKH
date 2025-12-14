using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;
using QLNCKH_HocVien.Helpers;
using QLNCKH_HocVien.Repositories;
using System.Security.Claims;

namespace QLNCKH_HocVien.Controllers
{
    /// <summary>
    /// Controller quản lý người dùng (Admin only)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(IUnitOfWork unitOfWork, ApplicationDbContext context, ILogger<UserManagementController> logger)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách tất cả người dùng (có pagination)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<object>>> GetAllUsers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? role = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                IQueryable<NguoiDung> query = _context.NguoiDungs;

                // Filter by search
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(u => 
                        u.TenDangNhap.Contains(search) || 
                        u.HoTen.Contains(search));
                }

                // Filter by role
                if (!string.IsNullOrWhiteSpace(role))
                {
                    query = query.Where(u => u.VaiTro == role);
                }

                // Filter by IsActive
                if (isActive.HasValue)
                {
                    query = query.Where(u => u.IsActive == isActive.Value);
                }

                // Order by NgayTao descending
                query = query.OrderByDescending(u => u.NgayTao);

                var totalCount = await query.CountAsync();
                var users = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        TenDangNhap = u.TenDangNhap,
                        HoTen = u.HoTen,
                        VaiTro = u.VaiTro,
                        IsActive = u.IsActive,
                        NgayTao = u.NgayTao
                    })
                    .ToListAsync();

                var paginatedResult = PaginatedResult<UserDto>.Create(users, totalCount, page, pageSize);
                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách người dùng");
                return BadRequest(ApiResult.Fail("Lỗi khi lấy danh sách người dùng"));
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết một người dùng
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<object>>> GetUserById(int id)
        {
            try
            {
                var user = await _unitOfWork.NguoiDungs.GetByIdAsync(id);
                if (user == null)
                    return NotFound(ApiResult.Fail("Không tìm thấy người dùng"));

                // Không trả về mật khẩu
                var userDto = new
                {
                    user.Id,
                    user.TenDangNhap,
                    user.HoTen,
                    user.VaiTro,
                    user.IsActive,
                    user.NgayTao
                };

                return Ok(ApiResult<object>.Ok(userDto, "Lấy thông tin người dùng thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin người dùng {UserId}", id);
                return BadRequest(ApiResult<object>.Fail("Lỗi khi lấy thông tin người dùng"));
            }
        }

        /// <summary>
        /// Cập nhật role của người dùng
        /// </summary>
        [HttpPut("{id}/role")]
        public async Task<ActionResult<ApiResult>> UpdateUserRole(int id, [FromBody] UpdateRoleRequest request)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                
                // Không cho phép tự thay đổi role của chính mình
                if (id == currentUserId)
                    return this.BadRequestResult("Bạn không thể thay đổi quyền của chính mình");

                var user = await _unitOfWork.NguoiDungs.GetByIdAsync(id);
                if (user == null)
                    return NotFound(ApiResult.Fail("Không tìm thấy người dùng"));

                // Validate role
                var validRoles = new[] { "Admin", "User", "GiaoVien" };
                if (!validRoles.Contains(request.VaiTro))
                    return BadRequest(ApiResult.Fail("Vai trò không hợp lệ"));

                // Không cho phép thay đổi role của Admin khác (chỉ có thể thay đổi User/GiaoVien thành Admin)
                // Nhưng cho phép Admin thay đổi role của Admin khác
                user.VaiTro = request.VaiTro;
                _unitOfWork.NguoiDungs.Update(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Admin {AdminId} đã cập nhật role của user {UserId} thành {Role}", 
                    currentUserId, id, request.VaiTro);

                return Ok(ApiResult.Ok("Cập nhật quyền thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật role của user {UserId}", id);
                return BadRequest(ApiResult.Fail("Lỗi khi cập nhật quyền"));
            }
        }

        /// <summary>
        /// Cập nhật trạng thái kích hoạt của người dùng
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<ActionResult<ApiResult>> UpdateUserStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                
                // Không cho phép tự vô hiệu hóa chính mình
                if (id == currentUserId && !request.IsActive)
                    return BadRequest(ApiResult.Fail("Bạn không thể vô hiệu hóa chính mình"));

                var user = await _unitOfWork.NguoiDungs.GetByIdAsync(id);
                if (user == null)
                    return NotFound(ApiResult.Fail("Không tìm thấy người dùng"));

                user.IsActive = request.IsActive;
                _unitOfWork.NguoiDungs.Update(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Admin {AdminId} đã {(Action)} user {UserId}", 
                    currentUserId, request.IsActive ? "kích hoạt" : "vô hiệu hóa", id);

                return Ok(ApiResult.Ok($"{(request.IsActive ? "Kích hoạt" : "Vô hiệu hóa")} tài khoản thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái user {UserId}", id);
                return BadRequest(ApiResult.Fail("Lỗi khi cập nhật trạng thái"));
            }
        }

        /// <summary>
        /// Xóa người dùng
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteUser(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                
                // Không cho phép tự xóa chính mình
                if (id == currentUserId)
                    return BadRequest(ApiResult.Fail("Bạn không thể xóa chính mình"));

                var user = await _unitOfWork.NguoiDungs.GetByIdAsync(id);
                if (user == null)
                    return NotFound(ApiResult.Fail("Không tìm thấy người dùng"));

                // Không cho phép xóa Admin khác (chỉ có thể xóa User/GiaoVien)
                if (user.VaiTro == "Admin")
                    return BadRequest(ApiResult.Fail("Không thể xóa tài khoản Admin"));

                _unitOfWork.NguoiDungs.Remove(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Admin {AdminId} đã xóa user {UserId}", currentUserId, id);

                return Ok(ApiResult.Ok("Xóa tài khoản thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa user {UserId}", id);
                return BadRequest(ApiResult.Fail("Lỗi khi xóa tài khoản"));
            }
        }

        /// <summary>
        /// Lấy thống kê người dùng
        /// </summary>
        [HttpGet("stats")]
        public async Task<ActionResult<ApiResult<object>>> GetUserStats()
        {
            try
            {
                var allUsers = await _unitOfWork.NguoiDungs.GetAllAsync();
                object stats = new
                {
                    Total = allUsers.Count(),
                    Active = allUsers.Count(u => u.IsActive),
                    Inactive = allUsers.Count(u => !u.IsActive),
                    Admin = allUsers.Count(u => u.VaiTro == "Admin"),
                    GiaoVien = allUsers.Count(u => u.VaiTro == "GiaoVien"),
                    User = allUsers.Count(u => u.VaiTro == "User")
                };

                return Ok(ApiResult<object>.Ok(stats, "Lấy thống kê người dùng thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thống kê người dùng");
                return BadRequest(ApiResult<object>.Fail("Lỗi khi lấy thống kê"));
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
    /// Request model để cập nhật role
    /// </summary>
    public class UpdateRoleRequest
    {
        public string VaiTro { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model để cập nhật status
    /// </summary>
    public class UpdateStatusRequest
    {
        public bool IsActive { get; set; }
    }
}

