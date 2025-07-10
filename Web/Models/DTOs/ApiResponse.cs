namespace Web.Models.DTOs;

public class ApiResponse<T>
{
    public int Status { get; set; }
    public T? Data { get; set; }
    public ErrorInfo? ErrorInfo { get; set; }
}