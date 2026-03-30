using FirstAPI.DTO;
using FirstAPI.DTO.Transactions;
using FirstAPI.Models;
using FirstAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        public readonly TransactionService _service;

        public TransactionsController(TransactionService service)
        {
            _service = service;
        }

        [HttpGet("GetAllTransactions")]
        public Task<APIResponse<List<TransactionResponse>>> GetAll()
        {
            var response = _service.GetAllTransactions();
            return response;
        }

        [HttpPost]
        public Task<APIResponse<Transaction>> Post(TransactionRequest req)
        {
            var response = _service.PostTransaction(req);
            return response;
        }

        [HttpPut("{id}")]
        public Task<APIResponse<Transaction>> Update(int id, TransactionRequest req)
        {
            var response = _service.UpdateTransaction(id, req);
            return response;
        }

        [HttpDelete("{id}")]
        public Task<APIResponse<Transaction>> Delete(int id)
        {
            var response = _service.DeleteTransaction(id);
            return response;
        }

        [HttpGet("Category-Summary")]
        public Task<APIResponse<List<TransactionCategorySummary>>> GetByCategory()
        {
            var response = _service.GetByCategory();
            return response;
        }
    }
}
