using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using try_blazor_wasm_policy_based_auth;
using try_blazor_wasm_policy_based_auth.Auth;
using try_blazor_wasm_policy_based_auth.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();

if (builder.HostEnvironment.IsEnvironment("DummyAuth"))
{
    builder.Services.AddScoped<DummyAuthenticationStateProvider>();
    builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<DummyAuthenticationStateProvider>());
    builder.Services.AddScoped<IAuthService, DummyAuthService>();
}
else
{
    builder.Services.AddScoped<IAuthService, MsalAuthService>();
    builder.Services.AddMsalAuthentication(options =>
    {
        builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
        options.ProviderOptions.LoginMode = "redirect"; // ⬅️ 既定は "popup"
    });
}

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
});

await builder.Build().RunAsync();
