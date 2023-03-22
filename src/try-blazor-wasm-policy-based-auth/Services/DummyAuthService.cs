using try_blazor_wasm_policy_based_auth.Auth;

namespace try_blazor_wasm_policy_based_auth.Services;

public class DummyAuthService : IAuthService
{
    private readonly DummyAuthenticationStateProvider _authenticationStateProvider;

    public DummyAuthService(DummyAuthenticationStateProvider authenticationStateProvider) =>
        this._authenticationStateProvider = authenticationStateProvider;

    /// <inheritdoc />
    public async Task LoginAsync() =>
        await this._authenticationStateProvider.NotifyLoginAsync().ConfigureAwait(false);

    /// <inheritdoc />
    public async Task LogoutAsync() =>
        await this._authenticationStateProvider.NotifyLogoutAsync().ConfigureAwait(false);
}