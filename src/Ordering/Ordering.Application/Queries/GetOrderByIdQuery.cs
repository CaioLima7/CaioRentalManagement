using MediatR;
using Ordering.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderResponse>
    {
        public string Id { get; set; }

        public GetOrderByIdQuery(string id)
        {
            Id = id;
        }
    }
}
