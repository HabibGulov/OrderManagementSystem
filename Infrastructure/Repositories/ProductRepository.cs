using Dapper;
using Npgsql;

public class ProductRepository : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<Product>(SqlCommands.GetAll);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении всех продуктов", ex);
            }
        }
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QuerySingleOrDefaultAsync<Product>(SqlCommands.GetById, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении продукта с ID {id}", ex);
            }
        }
    }

    public async Task<bool> CreateAsync(Product product)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync(SqlCommands.Create, product) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании продукта", ex);
            }
        }
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync(SqlCommands.Update, product) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при обновлении продукта", ex);
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
                throw new Exception($"Ошибка при удалении продукта с ID {id}", ex);
            }
        }
    }

    public async Task<IEnumerable<Product>> GetOutOfStockProductsAsync()
    {
        try
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(SqlCommands.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Product>(SqlCommands.GetOutOfStockProducts);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при получении продуктов с нулевым количеством на складе", ex);
        }
    }

    public async Task<Product?> GetMostPopularProductAsync()
    {
        using (var connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Product>(SqlCommands.GetMostPopularProduct);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении самого популярного продукта", ex);
            }
        }
    }

    public async Task<IEnumerable<Order>> GetOrdersForProductAsync(Guid productId)
    {
        using (var connection = new NpgsqlConnection(SqlCommands.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<Order>(SqlCommands.GetOrdersForProduct, new { productId });;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении заказов для продукта", ex);
            }
        }
    }
}
file class SqlCommands
{
    public const string ConnectionString = "Server=localhost;Port=5432;Database=marketplace_db;Username=postgres;Password=12345";
    public const string GetAll = "SELECT id, name, price, stock_quantity as stockquantity, created_at as createdat FROM products";
    public const string GetById = "SELECT id, name, price, stock_quantity as stockquantity, created_at as createdat FROM products WHERE id = @Id";
    public const string Create = "INSERT INTO products (id, name, price, stock_quantity, created_at) VALUES (@Id, @Name, @Price, @StockQuantity, @CreatedAt)";
    public const string Update = "UPDATE products SET name = @Name, price = @Price, stock_quantity = @StockQuantity WHERE id = @Id";
    public const string Delete = "DELETE FROM products WHERE id = @Id";
    public const string GetOutOfStockProducts = "SELECT * FROM products WHERE stock_quantity = 0";
    public const string GetMostPopularProduct = @"SELECT p.id, p.name, p.price, p.stock_quantity as stockquantity, p.created_at as createdat
                                                FROM products p
                                                JOIN order_items oi ON p.id = oi.product_id
                                                GROUP BY p.id
                                                ORDER BY SUM(oi.quantity) DESC  
                                                LIMIT 1;";
    public const string GetOrdersForProduct = @"SELECT o.id, o.customer_id as customerid, o.total_amount as totalamount, o.order_date as orderdate, o.status, o.created_at as createdat  FROM orders o
                                            JOIN order_items oi ON o.id = oi.order_id
                                            WHERE oi.product_id = @productId;";
}
