using System;
using FirstAPI.Data;
using FirstAPI.DTO;
using FirstAPI.DTO.Transactions;
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

    public async Task<APIResponse<List<TransactionResponse>>> GetAllTransactions()
    {
        var result = await _db.Transactions.Include(t => t.Category).Include(t => t.Account).Select(t => new TransactionResponse
        {
            TransactionUID = t.TransactionUID,
            Date = t.Date,
            Description = t.Description,
            Amount = t.Amount,
            CategoryId = t.CategoryUID,
            Category = t.Category.CategoryName,
            AccountId = t.AccountUID,
            Account = t.Account.AccountName
        }).ToListAsync();

        if (result.Count == 0)
        {
            return new APIResponse<List<TransactionResponse>> { Data = null, Message = "No Transaction found", StatusCode = 200 };
        }

        return new APIResponse<List<TransactionResponse>> { Data = result, Message = "Transactions fetched successfully", StatusCode = 200 };
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

    public async Task<APIResponse<Transaction>> UpdateTransaction(int id, TransactionRequest req)
    {
        if (req == null)
        {
            return new APIResponse<Transaction> { StatusCode = 400, Message = "You must provide the details", Data = null };
        }

        var transaction = await _db.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return new APIResponse<Transaction> { StatusCode = 404, Message = "Transaction Not found", Data = null };
        }

        transaction.Amount = req.Amount;
        transaction.CategoryUID = req.CategoryUID;
        transaction.AccountUID = req.AccountUID;
        transaction.Date = req.Date;
        transaction.Description = req.Description;

        await _db.SaveChangesAsync();

        return new APIResponse<Transaction>
        {
            StatusCode = 201,
            Message = "Transaction Updated successfully",
            Data = transaction
        };
    }

    public async Task<APIResponse<Transaction>> DeleteTransaction(int id)
    {
        var transaction = await _db.Transactions.FindAsync(id);

        if (transaction == null)
            return new APIResponse<Transaction> { StatusCode = 404, Message = "Transaction not found", Data = null };

        _db.Transactions.Remove(transaction);
        await _db.SaveChangesAsync();

        return new APIResponse<Transaction> { StatusCode = 200, Message = "Transaction deleted successfully", Data = transaction };
    }

    public async Task<APIResponse<List<TransactionCategorySummary>>> GetByCategory()
    {
        var summary = await _db.Transactions.GroupBy(x => x.Category.CategoryName).Select(g => new TransactionCategorySummary
        {
            CategoryName = g.Key,
            TotalAmount = g.Sum(x => x.Amount)
        }).ToListAsync();

        return new APIResponse<List<TransactionCategorySummary>>{ StatusCode = 200, Message= "Summary Retrieved", Data= summary};
    }
}
