using Dapper;
using Npgsql;

public class OrderItemRepository : IOrderItemRepository
{
    public async Task<IEnumerable<OrderItem>> GetAllAsync()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<OrderItem>(SqlCommands.GetAll);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении всех элементов заказа", ex);
            }
        }
    }

    public async Task<OrderItem?> GetByIdAsync(Guid id)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QuerySingleOrDefaultAsync<OrderItem>(SqlCommands.GetById, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении элемента заказа с ID {id}", ex);
            }
        }
    }

    public async Task<bool> CreateAsync(OrderItem orderItem)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync(SqlCommands.Create, orderItem) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании элемента заказа", ex);
            }
        }
    }

    public async Task<bool> UpdateAsync(OrderItem orderItem)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync(SqlCommands.Update, orderItem) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при обновлении элемента заказа", ex);
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
                throw new Exception($"Ошибка при удалении элемента заказа с ID {id}", ex);
            }
        }
    }

}
file class SqlCommands
{
    public const string ConnectionString = "Server=localhost;Port=5432;Database=marketplace_db;Username=postgres;Password=12345";
    public const string GetAll = "SELECT id, order_id as OrderId, product_id as ProductId, quantity, price, created_at as CreatedAt FROM order_items";
    public const string GetById = "SELECT id, order_id as OrderId, product_id as ProductId, quantity, price, created_at as CreatedAt FROM order_items WHERE id = @Id";
    public const string Create = "INSERT INTO order_items (id, order_id, product_id, quantity, price, created_at) VALUES (@Id, @OrderId, @ProductId, @Quantity, @Price, @CreatedAt)";
    public const string Update = "UPDATE order_items SET order_id = @OrderId, product_id = @ProductId, quantity = @Quantity, price = @Price WHERE id = @Id";
    public const string Delete = "DELETE FROM order_items WHERE id = @Id";
}