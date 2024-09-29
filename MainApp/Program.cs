using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Customer
app.MapGet("api/Customer/Get", async (ICustomerRepository customerRepository) =>
{
    return await customerRepository.GetAllAsync();
});
app.MapPost("api/Customer", async (ICustomerRepository customerRepository, Customer customer) =>
{
    return await customerRepository.CreateAsync(customer);
});
app.MapPut("api/Customer", async (ICustomerRepository customerRepository, Customer customer) =>
{
    return await customerRepository.UpdateAsync(customer);
});
app.MapDelete("api/Customer", async (ICustomerRepository customerRepository, Guid id) =>
{
    return await customerRepository.DeleteAsync(id);
});
app.MapGet("api/Customer{id}", async (ICustomerRepository customerRepository, Guid id) =>
{
    return await customerRepository.GetByIdAsync(id);
});
app.MapGet("api/Customers/Orders", async (ICustomerRepository customerRepository,  [FromQuery] DateTime startDate, [FromQuery] DateTime endDate) =>
{
    return await customerRepository.GetCustomersWithOrdersByDateRangeAsync(startDate, endDate);
});
app.MapGet("/api/Customers/Statistics", async (ICustomerRepository repository) =>
{
    return await repository.GetCustomerStatisticsAsync();
});
#endregion

#region Order
app.MapGet("api/Order", async (IOrderRepository orderRepository) =>
{
    return await orderRepository.GetAllAsync();
});
app.MapPost("api/Order", async (IOrderRepository orderRepository, Order order) =>
{
    return await orderRepository.CreateAsync(order);
});
app.MapPut("api/Order", async (IOrderRepository orderRepository, Order order) =>
{
    return await orderRepository.UpdateAsync(order);
});
app.MapDelete("api/Order", async (IOrderRepository orderRepository, Guid id) =>
{
    return await orderRepository.DeleteAsync(id);
});
app.MapGet("api/Order{id}", async (IOrderRepository orderRepository, Guid id) =>
{
    return await orderRepository.GetByIdAsync(id);
});
app.MapGet("/api/Orders/Details", async (IOrderRepository repository) =>
{
    return await repository.GetAllOrdersWithDetailsAsync();
});
app.MapGet("/api/OrdersByStatusAndDate", async (string status, DateTime startDate, DateTime endDate, IOrderRepository repository) =>
{
    return await repository.GetFilteredOrdersAsync(status, startDate, endDate);
});
app.MapGet("/api/Orders/Sales-Statistics", async (int month, int year, IOrderRepository repository) =>
{
    return await repository.GetSalesStatisticsAsync(month, year);
});
app.MapGet("/api/Orders/Products-Summary", async (IOrderRepository repository) =>
{
    return await repository.GetProductSalesSummaryAsync();
});
#endregion

#region OrderItem
app.MapGet("api/OrderItem", async (IOrderItemRepository orderItemRepository) =>
{
    return await orderItemRepository.GetAllAsync();
});
app.MapPost("api/OrderItem", async (IOrderItemRepository orderItemRepository, OrderItem orderItem) =>
{
    return await orderItemRepository.CreateAsync(orderItem);
});
app.MapPut("api/OrderItem", async (IOrderItemRepository orderItemRepository, OrderItem orderItem) =>
{
    return await orderItemRepository.UpdateAsync(orderItem);
});
app.MapDelete("api/OrderItem", async (IOrderItemRepository orderItemRepository, Guid id) =>
{
    return await orderItemRepository.DeleteAsync(id);
});
app.MapGet("api/FOrderItem{id}", async (IOrderItemRepository orderItemRepository, Guid id) =>
{
    return await orderItemRepository.GetByIdAsync(id);
});
#endregion

#region Product
app.MapGet("api/Product", async (IProductRepository productRepository) =>
{
    return await productRepository.GetAllAsync();
});
app.MapPost("api/Product", async (IProductRepository productRepository, Product product) =>
{
    return await productRepository.CreateAsync(product);
});
app.MapPut("api/Product", async (IProductRepository productRepository, Product product) =>
{
    return await productRepository.UpdateAsync(product);
});
app.MapDelete("api/Product", async (IProductRepository productRepository, Guid id) =>
{
    return await productRepository.DeleteAsync(id);
});
app.MapGet("api/Product{id}", async (IProductRepository productRepository, Guid id) =>
{
    return await productRepository.GetByIdAsync(id);
});
app.MapGet("api/Products/Out-Of-Stock", async (IProductRepository productRepository) =>
{
    return await productRepository.GetOutOfStockProductsAsync();
});
app.MapGet("/api/Products/Popular", async (IProductRepository repository) =>
{
    return await repository.GetMostPopularProductAsync();
});
app.MapGet("/api/Products/{ProductId}/Orders", async (Guid productId, IProductRepository repository) =>
{
    return await repository.GetOrdersForProductAsync(productId);
});
#endregion

app.Run();