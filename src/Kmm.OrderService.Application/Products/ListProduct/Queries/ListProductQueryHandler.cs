using Kmm.OrderService.Application.Common.Repositories;
using Kmm.OrderService.Application.Products.ListProduct.Specifications;
using Kmm.OrderService.Application.Products.Shared.Dtos;

namespace Kmm.OrderService.Application.Products.ListProduct.Queries
{
    public sealed class ListProductQueryHandler : IRequestHandler<ListProductQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ListProductQueryHandler(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(ListProductQuery request, CancellationToken cancellationToken)
        {
            var specification = new ListProductSpecification(request.Search);
            var products = await _productRepository.GetAsync(specification, cancellationToken);

            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}

