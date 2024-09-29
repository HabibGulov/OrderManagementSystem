public class CustomerStatistics
{
    public Guid Id { get; set; }
    public string FullName { get; set; }=null!;
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
}