using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace try_blazor_wasm_policy_based_auth;

/// <summary>
/// 承認ポリシーを提供します。
/// </summary>
public static class Policies
{
    /// <summary>管理者権限の承認ポリシー名を表します。</summary>
    public const string IsAdmin = "IsAdmin";

    /// <summary>
    /// 管理者権限の承認ポリシーを取得します。
    /// </summary>
    /// <returns><see cref="AuthorizationPolicy"/>。</returns>
     public static AuthorizationPolicy IsAdminPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireAssertion(context =>
            {
                var rolesString = context.User.Claims.FirstOrDefault(claim => claim.Type == "roles")?.Value;
                if (string.IsNullOrEmpty(rolesString))
                {
                    return false;
                }

                var roles = JsonSerializer.Deserialize<string[]>(rolesString);
                return roles?.Any(role => role == "admin") ?? false;
            })
            .Build();
}