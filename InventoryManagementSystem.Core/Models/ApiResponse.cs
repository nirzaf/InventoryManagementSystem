using System.Collections.Generic;

namespace InventoryManagementSystem.Core.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> CreateSuccess(T data) => new() { Success = true, Data = data };
    public static ApiResponse<T> CreateFailure(string message, List<string>? errors = null) => new() { Success = false, ErrorMessage = message, Errors = errors };
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse CreateSuccess() => new() { Success = true };
    public static ApiResponse CreateFailure(string message, List<string>? errors = null) => new() { Success = false, ErrorMessage = message, Errors = errors };
}
