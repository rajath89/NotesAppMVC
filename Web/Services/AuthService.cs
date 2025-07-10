using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Models;
using Web.Models.DTOs;
using Web.Models.Interfaces;

namespace Web.Services;

public class AuthService : IAuthService
    {
        private readonly IAuthApiService _authApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IAuthApiService authApiService, IHttpContextAccessor httpContextAccessor, ILogger<AuthService> logger)
        {
            _authApiService = authApiService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            try
            {
                var request = new LoginRequest
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var response = await _authApiService.LoginAsync(request);
                
                if (response.Status == 0)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.Email, model.Email)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await _httpContextAccessor.HttpContext!.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return false;
            }
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var request = new RegisterRequest
                {
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };

                var response = await _authApiService.RegisterAsync(request);
                return response.Status == 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return false;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                var response = await _authApiService.LogoutAsync();
                
                if (response.Status == 0)
                {
                    await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return false;
            }
        }
    }