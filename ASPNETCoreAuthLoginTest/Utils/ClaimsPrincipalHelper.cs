using  ASPNETCoreAuthLoginTest.Models;
using System.Security.Claims;

namespace ASPNETCoreAuthLoginTest.Utils
{
    public static class ClaimsPrincipalHelper
    {
        /// <summary>
        /// 安全地從 User 取得角色列舉
        /// </summary>
        public static bool TryGetUserRole(this ClaimsPrincipal user, out UserRole role)
        {
            role = default;

            var roleString = user.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrWhiteSpace(roleString))
                return false;

            if (Enum.TryParse<UserRole>(roleString, out var parsedRole)
                && Enum.IsDefined(typeof(UserRole), parsedRole))
            {
                role = parsedRole;
                return true;
            }

            return false;
        }
    }
}
