using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookStoreApp_Blazor_Server_UI.Providers;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Start with user being in a not logged in state:
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        var savedToken = await _localStorage.GetItemAsync<string>("accessToken");

        if (savedToken == null)
        {
            // No token, so return an authentication state with an empty claims principal:
            return new AuthenticationState(user);
        }

        // Decode the contents of the token so we can examine it
        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);

        // If token is expired, return not logged in:
        if (tokenContent.ValidTo < DateTime.Now)
        {
            return new AuthenticationState(user);
        }

        // Pull out the claims from the token:
        var claims = tokenContent.Claims;

        user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthentication"));
        
        return new AuthenticationState(new ClaimsPrincipal(user));
    }

    public async Task LoggedIn()
    {
        // Get the token info (similar to the GetAuthenticationStateAsync method):
        var savedToken = await _localStorage.GetItemAsync<string>("accessToken");
        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
        var claims = tokenContent.Claims;
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthentication"));
        var authSate = Task.FromResult(new AuthenticationState(user));

        // Notify the app of the change to user authentication state:
        NotifyAuthenticationStateChanged(authSate);
    }

    public async Task LoggedOut()
    {
        // Non logged in state:
        await _localStorage.RemoveItemAsync("accessToken");
        var nobody = new ClaimsPrincipal(new ClaimsIdentity());
        var authSate = Task.FromResult(new AuthenticationState(nobody));

        // Notify the app of the change to user authentication state:
        NotifyAuthenticationStateChanged(authSate);
    }

}
