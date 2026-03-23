using System;
using FirstAPI.Data;
using FirstAPI.DTO;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Services;

public class TransactionService
{
    public readonly AppDBContext _db;
    public TransactionService(AppDBContext db)
    {
        _db = db;
    }

    public async Task<APIResponse<List<Transaction>>> GetAllTransactions()
    {
        var result = await _db.Transactions.ToListAsync();
        if (result == null)
        {
            return new APIResponse<List<Transaction>> { Data = null, Message = "No Transaction found", StatusCode = 200 };
        }

        return new APIResponse<List<Transaction>> { Data = result, Message = "Transactions fetched successfully", StatusCode = 200 };
    }

    public async Task<APIResponse<Transaction>> PostTransaction(TransactionRequest req)
    {
        if (req == null)
        {
            return new APIResponse<Transaction> { StatusCode = 400, Message = "You must provide the details", Data = null };
        }

        // Create new transaction
        var transaction = new Transaction()
        {
            AccountUID = req.AccountUID,
            CategoryUID = req.CategoryUID,
            Amount = req.Amount,
            Date = req.Date,
            Description = req.Description
        };

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

         return new APIResponse<Transaction>
        {
            StatusCode = 201,
            Message = "Transaction added successfully",
            Data = transaction
        };
    }
}
