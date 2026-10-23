namespace WebApplication5.DTOs;

public record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount);
