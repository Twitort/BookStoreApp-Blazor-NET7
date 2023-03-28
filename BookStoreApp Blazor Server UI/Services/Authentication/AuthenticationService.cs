using Blazored.LocalStorage;
using BookStoreApp_Blazor_Server_UI.Providers;
using BookStoreApp_Blazor_Server_UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp_Blazor_Server_UI.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> AuthenticateAsync(LoginUserDTO loginModel)
    {
        var response = await _httpClient.LoginAsync(loginModel);

        // Store token
        await _localStorage.SetItemAsStringAsync("accessToken", response.Token);

        // Change auth state of app
        await ((ApiAuthenticationStateProvider)_authStateProvider).LoggedIn();

        return true;
    }

    public async Task Logout()
    {
        await ((ApiAuthenticationStateProvider)_authStateProvider).LoggedOut();
    }
}
