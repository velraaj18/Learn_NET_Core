using FirstAPI.DTO;
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
        public Task<APIResponse<List<Transaction>>> GetAll()
        {
            var response = _service.GetAllTransactions();
            return response;
        }

        public Task<APIResponse<Transaction>> Post(TransactionRequest req)
        {
            var response = _service.PostTransaction(req);
            return response;
        }

    }
}
