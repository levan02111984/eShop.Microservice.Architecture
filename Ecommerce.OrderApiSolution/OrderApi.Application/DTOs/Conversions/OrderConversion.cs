using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDTO order) => new()
        {
            Id = order.Id,
            ClientId = order.ClientId,
            ProductId = order.ProductId,
            OrderedDate = order.OrderedDate,
            PurchaseQuantity = order.PurchaseQuantity
        };

        public static (OrderDTO? , IEnumerable<OrderDTO>?) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            //Return single
            if(order is not null || orders is null)
            {
                var currentOrder = new OrderDTO(
                    order!.Id,
                    order.ClientId,
                    order.ProductId,
                    order.PurchaseQuantity,
                    order.OrderedDate
                );

                return (currentOrder, null);
            }


            //Return list
            if(order is null || orders is not null)
            {
                var currentOrders = orders.Select(p =>
                        new OrderDTO(
                            p.Id,
                            p.ProductId,
                            p.ClientId,
                            p.PurchaseQuantity,
                            p.OrderedDate
                    )).ToList();

                return (null, currentOrders);
            }

            return (null,null);
        }
    }
}
