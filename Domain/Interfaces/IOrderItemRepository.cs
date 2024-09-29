public interface IOrderItemRepository
{
    Task<IEnumerable<OrderItem>> GetAllAsync();
    Task<OrderItem?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(OrderItem orderItem);
    Task<bool> UpdateAsync(OrderItem orderItem);
    Task<bool> DeleteAsync(Guid id);
}