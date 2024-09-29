public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(Customer customer);
    Task<bool> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<CustomerOrderInfo>> GetCustomersWithOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<CustomerStatistics>> GetCustomerStatisticsAsync();
}
