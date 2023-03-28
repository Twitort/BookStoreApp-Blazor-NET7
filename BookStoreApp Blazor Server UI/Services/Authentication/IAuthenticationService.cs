using BookStoreApp_Blazor_Server_UI.Services.Base;

namespace BookStoreApp_Blazor_Server_UI.Services.Authentication;

public interface IAuthenticationService
{
    Task<bool> AuthenticateAsync(LoginUserDTO loginModel);

    public Task Logout();
}
