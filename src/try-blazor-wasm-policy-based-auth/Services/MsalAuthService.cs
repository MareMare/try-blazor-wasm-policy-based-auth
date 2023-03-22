using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace try_blazor_wasm_policy_based_auth.Services;

public class MsalAuthService : IAuthService
{
    private readonly NavigationManager _navigation;
    private readonly IOptionsSnapshot<RemoteAuthenticationOptions<ApiAuthorizationProviderOptions>> _options;

    public MsalAuthService(NavigationManager navigation, IOptionsSnapshot<RemoteAuthenticationOptions<ApiAuthorizationProviderOptions>> options)
    {
        this._navigation = navigation;
        this._options = options;
    }

    /// <inheritdoc />
    public Task LoginAsync()
    {
        var loginPath = this._options.Get(Options.DefaultName).AuthenticationPaths.LogInPath;
        this._navigation.NavigateToLogin(loginPath);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task LogoutAsync()
    {
        var logoutPath = this._options.Get(Options.DefaultName).AuthenticationPaths.LogOutPath;
        this._navigation.NavigateToLogout(logoutPath);
        return Task.CompletedTask;
    }
}