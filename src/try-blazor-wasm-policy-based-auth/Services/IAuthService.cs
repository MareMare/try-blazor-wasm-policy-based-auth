namespace try_blazor_wasm_policy_based_auth.Services;

public interface IAuthService
{
    Task LoginAsync();
    Task LogoutAsync();
}