using Kmm.OrderService.Application.Common.Data;
using Kmm.OrderService.Application.Common.Exceptions;
using Kmm.OrderService.Application.Common.Repositories;
using Kmm.OrderService.Application.Orders.CreateOrder.Dtos;
using Kmm.OrderService.Application.Shared.Specifications;
using Kmm.OrderService.Domain.Entities;

namespace Kmm.OrderService.Application.Orders.CreateOrder.Commands;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();

            var specification = new GetProductSpecification(productIds);
            var products = await _productRepository.GetAsync(specification, cancellationToken);

            if (products.Count != productIds.Count)
            {
                throw new NotFoundException("One or more products were not found.");
            }

            var productById = products.ToDictionary(p => p.Id);

            var order = new Order(request.CustomerId, request.Currency);

            foreach (var item in request.Items)
            {
                var product = productById[item.ProductId];
                order.AddItem(product, item.Quantity);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            _orderRepository.Insert(order);
            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<CreateOrderDto>(order);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
