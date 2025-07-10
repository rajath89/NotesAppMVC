namespace Web.Models.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginViewModel model);
    Task<bool> RegisterAsync(RegisterViewModel model);
    Task<bool> LogoutAsync();
}