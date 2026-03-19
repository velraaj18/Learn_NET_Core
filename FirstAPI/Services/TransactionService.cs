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
            return new APIResponse<List<Transaction>>{Data = null, Message="No Transaction found", StatusCode= 200};
        }

        return new APIResponse<List<Transaction>>{Data = result, Message="Transactions fetched successfully", StatusCode= 200};
    }
}
