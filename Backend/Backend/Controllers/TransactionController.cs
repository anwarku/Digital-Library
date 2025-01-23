using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public ActionResult<List<Transaction>> GetAllTransactions()
        {
            var allTransactions = _transactionService.GetAllTransactions();
            return Ok(allTransactions);
        }

        [HttpGet]
        [Route("{transactionId}")]
        public ActionResult<TransactionDto> GetTransaction(string transactionId)
        {
            try
            {
                var data = _transactionService.GetTransactionById(transactionId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("last")]
        public ActionResult<Transaction> GetLastTrancastion()
        {
            return Ok(_transactionService.GetLastTransaction());
        }
    }
}
