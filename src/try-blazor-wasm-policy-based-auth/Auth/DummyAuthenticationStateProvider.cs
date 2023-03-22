using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace try_blazor_wasm_policy_based_auth.Auth;

public class DummyAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string UserNameKey = "usernameKey";
    private readonly ILocalStorageService _localStorage;

    public DummyAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    /// <inheritdoc />
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var userName = await _localStorage.GetItemAsync<string>(nameof(UserNameKey)).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(userName))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var userRoles = JsonSerializer.Serialize(new[] { "admin", });
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, userName),
            new ("roles", userRoles),
        };

        var anonymous = new ClaimsPrincipal(new ClaimsIdentity(claims, "fake auth"));
        return new AuthenticationState(anonymous);
    }

    public async Task NotifyLoginAsync()
    {
        var userName = "DEBUG USER";
        await this._localStorage.SetItemAsync(nameof(UserNameKey), userName).ConfigureAwait(false);
        this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
    }

    public async Task NotifyLogoutAsync()
    {
        await this._localStorage.RemoveItemAsync(nameof(UserNameKey)).ConfigureAwait(false);
        this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
    }
}