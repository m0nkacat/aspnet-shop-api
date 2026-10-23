namespace WebApplication5.Services;

public enum ServiceStatus
{
    Success,
    NotFound,
    Invalid,
    Conflict
}

public class ServiceResult
{
    public ServiceStatus Status { get; init; }
    public string? Error { get; init; }
    public bool IsSuccess => Status == ServiceStatus.Success;

    public static ServiceResult Ok() => new() { Status = ServiceStatus.Success };

    public static ServiceResult Fail(ServiceStatus status, string error) =>
        new() { Status = status, Error = error };
}

public class ServiceResult<T> : ServiceResult
{
    public T? Value { get; init; }

    public static ServiceResult<T> Ok(T value) => new() { Status = ServiceStatus.Success, Value = value };

    public new static ServiceResult<T> Fail(ServiceStatus status, string error) =>
        new() { Status = status, Error = error };
}
