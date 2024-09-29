public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<Product>> GetOutOfStockProductsAsync();
    Task<Product?> GetMostPopularProductAsync();
    Task<IEnumerable<Order>> GetOrdersForProductAsync(Guid productId);
}
