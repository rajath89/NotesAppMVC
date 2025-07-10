namespace Web.Models.DTOs;

public class AuthResponse
{
    public int Status { get; set; }
    public ErrorInfo? ErrorInfo { get; set; }
}