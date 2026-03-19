namespace Kmm.OrderService.Application.Shared.Dtos;

public record PaginatedListDto<T>
{
    public required IEnumerable<T> Items { get; init; }
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required int TotalItems { get; init; }
    public required int TotalPages { get; init; }
}
