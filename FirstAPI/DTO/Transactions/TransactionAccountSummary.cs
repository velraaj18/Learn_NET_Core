using System;

namespace FirstAPI.DTO.Transactions;

public class TransactionAccountSummary
{
    public string AccountName {get; set;}
    public string CategoryName {get; set;}
    public decimal TotalAmount {get; set;}

}
