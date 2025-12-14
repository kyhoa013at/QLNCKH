using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Client.Services;

namespace QLNCKH_HocVien.Client.Shared
{
    public static class AuthorizationHelper
    {
        public static bool IsAdmin(UserInfo? user) => user?.VaiTro == "Admin";
        public static bool IsGiaoVien(UserInfo? user) => user?.VaiTro == "GiaoVien";
        public static bool IsHocVien(UserInfo? user) => user?.VaiTro == "User";
        
        public static bool HasAnyRole(UserInfo? user, params string[] roles)
        {
            if (user == null) return false;
            return roles.Contains(user.VaiTro);
        }
    }
}

