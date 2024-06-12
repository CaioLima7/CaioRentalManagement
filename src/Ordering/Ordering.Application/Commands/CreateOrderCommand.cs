using Ordering.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using Ordering.Core.Entities;

namespace Ordering.Application.Commands
{
    public class CreateOrderCommand : IRequest<OrderResponse>
    {
        public string UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItems> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }

        public CreateOrderCommand(string userName, DateTime orderDate, List<OrderItems> orderItems, 
            decimal totalPrice)
        {
            UserName = userName;
            OrderDate = orderDate;
            OrderItems = orderItems;
            TotalPrice = totalPrice;
        }
    }
}
