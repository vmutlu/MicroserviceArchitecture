using DotnetMicroserviceArchitecture.Order.Core.Abstract;
using DotnetMicroserviceArchitecture.Order.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetMicroserviceArchitecture.Order.Domain.Aggregate
{
    public class Order : BaseEntity, IAggregateRoot
    {
        public DateTime InsertedDate { get; private set; }
        public Adress Adress { get; private set; }
        public string BuyerId { get; private set; }

        private readonly List<OrderItem> _orderItems; //okuma yazma işlemi property değil de buşekilde field üzerinde gerçekleştirilirse backing field olarak adlandırılır.

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order(string buyerId, Adress adress)
        {
            _orderItems = new();
            InsertedDate = DateTime.Now;
            BuyerId = buyerId;
            Adress = adress;
        }

        public void AddOrderItem(string orderId, string name, string uRL, decimal price)
        {
            var exits = _orderItems.Any(x => x.OrderId == orderId);
            if (!exits)
                _orderItems.Add(new OrderItem(orderId, name, uRL, price));
        }

        public decimal TotalPrice() => _orderItems.Sum(x => x.Price);
    }
}
