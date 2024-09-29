using Dapper;
using Npgsql;

public class CustomerRepository : ICustomerRepository
{
    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<Customer>(SqlCommands.GetAll);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении всех клиентов", ex);
            }
        }
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QuerySingleOrDefaultAsync<Customer>(SqlCommands.GetById, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении клиента с ID {id}", ex);
            }
        }
    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync(SqlCommands.Create, customer) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании клиента", ex);
            }
        }
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync(SqlCommands.Update, customer) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при обновлении клиента", ex);
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
                throw new Exception($"Ошибка при удалении клиента c ID {id}", ex);
            }
        }
    }

    public async Task<IEnumerable<CustomerOrderInfo>> GetCustomersWithOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<CustomerOrderInfo>(SqlCommands.GetCustomersWithOrdersByDate, new { StartDate = startDate, EndDate = endDate });
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при получении клиентов с заказами", ex);
        }
    }
    public async Task<IEnumerable<CustomerStatistics>> GetCustomerStatisticsAsync()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<CustomerStatistics>(SqlCommands.GetCustomerStatistics);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении статистики клиентов", ex);
            }
        }
    }
}
file class SqlCommands
{
    public const string ConnectionString = "Server=localhost;Port=5432;Database=marketplace_db;Username=postgres;Password=12345";
    public const string GetAll = "SELECT id, full_name as FullName, email, phone, created_at as CreatedAt FROM customers";
    public const string GetById = "SELECT id, full_name as FullName, email, phone, created_at as CreatedAt FROM customers WHERE id = @Id";
    public const string Create = "INSERT INTO customers (id, full_name, email, phone, created_at) VALUES (@Id, @FullName, @Email, @Phone, @CreatedAt)";
    public const string Update = "UPDATE customers SET full_name = @FullName, email = @Email, phone = @Phone WHERE id = @Id";
    public const string Delete = "DELETE FROM customers WHERE id = @Id";
    public const string GetCustomersWithOrdersByDate = @"SELECT c.id AS CustomerId, c.full_name AS FullName, c.email, c.phone, c.created_at AS CustomerCreatedAt, 
                                                            o.id AS OrderId, o.order_date AS OrderDate, o.total_amount AS TotalAmount, o.status AS Status, o.created_at AS OrderCreatedAt
                                                            FROM customers c
                                                            JOIN orders o ON c.id = o.customer_id
                                                            WHERE o.order_date BETWEEN @StartDate AND @EndDate";
    public const string GetCustomerStatistics = @"SELECT c.id, c.full_name as fullname, COUNT(o.id) AS totalorders, COALESCE(SUM(o.total_amount), 0) AS totalspent
                                                    FROM customers c
                                                    JOIN orders o ON c.id = o.customer_id
                                                    GROUP BY c.id";
}