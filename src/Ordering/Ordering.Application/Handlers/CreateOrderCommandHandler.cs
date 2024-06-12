using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                UserName = request.UserName,
                OrderDate = request.OrderDate,
                OrderItems = request.OrderItems,
                TotalPrice = request.TotalPrice
            };

            await _orderRepository.AddAsync(order);

            return new OrderResponse
            {
                Id = order.Id,
                UserName = order.UserName,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems,
                TotalPrice = order.TotalPrice
            };
        }
    }
}
