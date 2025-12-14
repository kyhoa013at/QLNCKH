using System.Security.Claims;

namespace QLNCKH_HocVien.Helpers
{
    /// <summary>
    /// Helper class để kiểm tra quyền người dùng
    /// </summary>
    public static class AuthorizationHelper
    {
        /// <summary>
        /// Kiểm tra user có role Admin không
        /// </summary>
        public static bool IsAdmin(ClaimsPrincipal? user)
        {
            return user?.IsInRole("Admin") == true;
        }

        /// <summary>
        /// Kiểm tra user có role GiaoVien không
        /// </summary>
        public static bool IsGiaoVien(ClaimsPrincipal? user)
        {
            return user?.IsInRole("GiaoVien") == true;
        }

        /// <summary>
        /// Kiểm tra user có role User (Học viên) không
        /// </summary>
        public static bool IsHocVien(ClaimsPrincipal? user)
        {
            return user?.IsInRole("User") == true;
        }

        /// <summary>
        /// Kiểm tra user có một trong các role được chỉ định
        /// </summary>
        public static bool HasAnyRole(ClaimsPrincipal? user, params string[] roles)
        {
            if (user == null) return false;
            return roles.Any(role => user.IsInRole(role));
        }

        /// <summary>
        /// Kiểm tra user có tất cả các role được chỉ định
        /// </summary>
        public static bool HasAllRoles(ClaimsPrincipal? user, params string[] roles)
        {
            if (user == null) return false;
            return roles.All(role => user.IsInRole(role));
        }

        /// <summary>
        /// Lấy role hiện tại của user
        /// </summary>
        public static string? GetCurrentRole(ClaimsPrincipal? user)
        {
            return user?.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}

