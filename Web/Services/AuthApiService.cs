using System.Text;
using System.Text.Json;
using Web.Models.DTOs;
using Web.Models.Interfaces;

namespace Web.Services;

public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthApiService> _logger;

        public AuthApiService(HttpClient httpClient, ILogger<AuthApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("/NotesApp/public/api/v1/Auth/login", content);
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return authResponse ?? new AuthResponse { Status = -1 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return new AuthResponse { Status = -1, ErrorInfo = new ErrorInfo { Code = "LOGIN_ERROR", Description = "Login failed" } };
            }
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("/NotesApp/public/api/v1/Auth/register", content);
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return authResponse ?? new AuthResponse { Status = -1 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return new AuthResponse { Status = -1, ErrorInfo = new ErrorInfo { Code = "REGISTER_ERROR", Description = "Registration failed" } };
            }
        }

        public async Task<AuthResponse> LogoutAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("/NotesApp/public/api/v1/Auth/logout", null);
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return authResponse ?? new AuthResponse { Status = -1 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return new AuthResponse { Status = -1, ErrorInfo = new ErrorInfo { Code = "LOGOUT_ERROR", Description = "Logout failed" } };
            }
        }
    }