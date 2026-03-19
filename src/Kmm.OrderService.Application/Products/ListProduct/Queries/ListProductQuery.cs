using Kmm.OrderService.Application.Products.Shared.Dtos;

namespace Kmm.OrderService.Application.Products.ListProduct.Queries;

public sealed record ListProductQuery(string? Search = null) : IRequest<List<ProductDto>>;

