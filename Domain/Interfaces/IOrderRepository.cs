public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(Order order);
    Task<bool> UpdateAsync(Order order);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<OrderDetail>> GetAllOrdersWithDetailsAsync();
    Task<IEnumerable<Order>> GetFilteredOrdersAsync(string status, DateTime startDate, DateTime endDate);
    Task<SalesStatistics?> GetSalesStatisticsAsync(int month, int year);
    Task<IEnumerable<ProductSalesSummary>> GetProductSalesSummaryAsync();
}
