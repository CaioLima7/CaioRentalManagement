using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllAsync();
            var response = new List<OrderResponse>();

            foreach (var order in orders)
            {
                response.Add(new OrderResponse
                {
                    Id = order.Id,
                    UserName = order.UserName,
                    OrderDate = order.OrderDate,
                    OrderItems = order.OrderItems,
                    TotalPrice = order.TotalPrice
                });
            }

            return response;
        }
    }
}
