using Kmm.OrderService.Application.Common.Data;
using Kmm.OrderService.Application.Common.Exceptions;
using Kmm.OrderService.Application.Common.Repositories;
using Kmm.OrderService.Application.Orders.ConfirmOrder.Dtos;
using Kmm.OrderService.Application.Shared.Specifications;
using Kmm.OrderService.Domain.Common.Exceptions;
using Kmm.OrderService.Domain.Enums;

namespace Kmm.OrderService.Application.Orders.ConfirmOrder.Commands;

public sealed class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, ConfirmOrderDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ConfirmOrderCommandHandler(
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

    public async Task<ConfirmOrderDto> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var getOrderByIdSpecification = new GetOrderByIdSpecification(request.Id);
            var order = await _orderRepository.GetFirstAsync(getOrderByIdSpecification, cancellationToken);
            NotFoundException.ThrowIfNull(order, "Order is required");

            if (order!.Status == EOrderStatus.Confirmed)
            {
                return _mapper.Map<ConfirmOrderDto>(order);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var productIds = order.OrderItems.Select(i => i.ProductId).Distinct().ToList();
            var getProductSpecification = new GetProductSpecification(productIds);
            var products = await _productRepository.GetAsync(getProductSpecification, cancellationToken);

            if (products.Count != productIds.Count)
            {
                throw new NotFoundException("One or more products were not found.");
            }

            var productById = products.ToDictionary(p => p.Id);

            foreach (var item in order.OrderItems)
            {
                var product = productById[item.ProductId];
                product.DecreaseStock(item.Quantity);
                _productRepository.Update(product); ;
            }

            order.Confirm();
            _orderRepository.Update(order);

            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<ConfirmOrderDto>(order);
        }
        catch (DbUpdateConcurrencyException)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw new BusinessValidationException("Stock was updated by another operation. Please retry.");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

