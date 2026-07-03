using OrderManagement.Application.Contracts.Responses;
using OrderManagement.Application.Mappings.Extensions;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Mappings
{
    public static class OrderMapping
    {
        public static OrderResponse ToResponse(this Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CreatedAt = order.CreatedAt,
                Total = order.Total,
                Status = order.Status.ToString(),

                Items = order.Items
                    .Select(i => new OrderItemResponse
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Total = i.Total
                    })
                    .ToList(),

                History = order.History
                    .OrderBy(h => h.ModificationDate)
                    .Select(h => new OrderHistoryResponse
                    {
                        Id = h.Id,
                        PreviousStatus = h.PreviousStatus.ToString(),
                        NewStatus = h.NewStatus.ToString(),
                        ChangedAt = h.ModificationDate,
                        Reason = h.Reason
                    })
                    .ToList()
            };
        }
    }
}
