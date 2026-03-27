public class TransactionResponse
{
    public int TransactionUID { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public string Account { get; set; }
}