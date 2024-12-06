using WebApplication1.Models;

namespace WebApplication1.Service;

public interface IOrderService
{
    Task<List<Order>> GetOrdersAsync();
    Task AddOrderAsync(Order order);
}