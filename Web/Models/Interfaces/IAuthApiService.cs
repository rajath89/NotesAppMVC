using Web.Models.DTOs;

namespace Web.Models.Interfaces;

public interface IAuthApiService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LogoutAsync();
}