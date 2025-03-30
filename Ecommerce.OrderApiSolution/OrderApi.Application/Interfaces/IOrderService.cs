using OrderApi.Application.DTOs;

namespace OrderApi.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId);
        Task<OrderDetailsDTO> GetOrderDetails(int orderId);
    }
}
