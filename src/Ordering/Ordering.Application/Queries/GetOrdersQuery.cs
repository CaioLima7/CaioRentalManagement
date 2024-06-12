using MediatR;
using Ordering.Application.Responses;
using System.Collections.Generic;

namespace Ordering.Application.Queries
{
    public class GetOrdersQuery : IRequest<List<OrderResponse>>
    {
    }
}
