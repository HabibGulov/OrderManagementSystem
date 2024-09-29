using Dapper;
using Npgsql;

public class OrderRepository : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<Order>(SqlCommands.GetAll);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении всех заказов", ex);
            }
        }
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QuerySingleOrDefaultAsync<Order>(SqlCommands.GetById, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении заказа с ID {id}", ex);
            }
        }
    }

    public async Task<bool> CreateAsync(Order order)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync(SqlCommands.Create, order) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании заказа", ex);
            }
        }
    }

    public async Task<bool> UpdateAsync(Order order)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync(SqlCommands.Update, order) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при обновлении заказа", ex);
            }
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync(SqlCommands.Delete, new { Id = id }) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении заказа с ID {id}", ex);
            }
        }
    }

    public async Task<IEnumerable<OrderDetail>> GetAllOrdersWithDetailsAsync()
    {
        try
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<OrderDetail>(SqlCommands.GetAllOrdersWithDetailsAsync);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при получении заказов с информацией о клиентах и товарах", ex);
        }
    }
    public async Task<IEnumerable<Order>> GetFilteredOrdersAsync(string status, DateTime startDate, DateTime endDate)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<Order>(SqlCommands.GetFilteredOrders, new { Status = status, StartDate = startDate, EndDate = endDate });
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении заказов по фильтру", ex);
            }
        }
    }

    public async Task<SalesStatistics?> GetSalesStatisticsAsync(int month, int year)
    {
        using (var connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<SalesStatistics>(SqlCommands.GetSalesStatistics, new { month, year });
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении статистики по продажам", ex);
            }
        }
    }
 
    public async Task<IEnumerable<ProductSalesSummary>> GetProductSalesSummaryAsync()
    {
        using (var connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<ProductSalesSummary>(SqlCommands.GetProductSalesSummary);

            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении информации о заказах с общей суммой по каждому продукту", ex);
            }
        }
    }

}
file class SqlCommands
{
    public const string ConnectionString = "Server=localhost;Port=5432;Database=marketplace_db;Username=postgres;Password=12345";
    public const string GetAll = "SELECT id, customer_id as customerid, total_amount as totalamount, order_date as orderdate, status, created_at as createdat FROM orders";
    public const string GetById = "SELECT id, customer_id as customerid, total_amount as totalamount, order_date as orderdate, status, created_at as createdat FROM orders WHERE id = @Id";
    public const string Create = "INSERT INTO orders (id, customer_id, total_amount, order_date, status, created_at) VALUES (@Id, @CustomerId, @TotalAmount, @OrderDate, @Status, @CreatedAt)";
    public const string Update = "UPDATE orders SET customer_id = @CustomerId, total_amount = @TotalAmount, order_date = @OrderDate, status = @Status WHERE id = @Id";
    public const string Delete = "DELETE FROM orders WHERE id = @Id";
    public const string GetAllOrdersWithDetailsAsync = @"SELECT o.id, o.order_date AS OrderDate, c.id as CustomerId, c.full_name AS CustomerFullName, c.email AS CustomerEmail, c.phone AS CustomerPhone, p.Id as ProductId,  p.name AS ProductName, oi.quantity AS Quantity, (oi.quantity * p.price) AS TotalPrice
                                                        FROM orders o
                                                        JOIN customers c ON o.customer_id = c.id
                                                        JOIN order_items oi ON o.id = oi.order_id
                                                        JOIN products p ON oi.product_id = p.id;";
    public const string GetFilteredOrders = @"SELECT id, customer_id as customerid, order_date as orderdate, status, total_amount as totalamount FROM orders WHERE status = @Status AND order_date BETWEEN @StartDate AND @EndDate";
    public const string GetSalesStatistics = @"SELECT SUM(oi.quantity) AS totalquantity, SUM(oi.quantity * p.price) AS totalsales
                                            FROM orders o
                                            JOIN order_items oi ON o.id = oi.order_id
                                            JOIN products p ON oi.product_id = p.id
                                            WHERE EXTRACT(MONTH FROM o.created_at) = @month AND EXTRACT(YEAR FROM o.created_at) = @year
                                            GROUP BY EXTRACT(MONTH FROM o.created_at), EXTRACT(YEAR FROM o.created_at);";
    public const string GetProductSalesSummary = @" SELECT p.id, p.name, SUM(oi.quantity * oi.price) AS totalsales
                                                FROM products p
                                                JOIN order_items oi ON p.id = oi.product_id
                                                JOIN orders o ON oi.order_id = o.id
                                                GROUP BY p.id, p.name;";
}
