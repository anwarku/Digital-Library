using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        [Route("borrowed")]
        public ActionResult<List<BorrowedTransactionDto>> GetBorrowedTransactions([FromQuery] int skip = 0, int limit = 5, string search = "") 
        {
            var borrowedTransactions = _transactionService.GetBorrowedTransactions(skip, limit, search);

            return Ok(new
            {
                limit, 
                skip, 
                total = 
                (search.IsNullOrEmpty() ? _transactionService.CountBorrowedTransactions() : _transactionService.CountBorrowedSearchTransactions(search)),
                data = borrowedTransactions
            });
        }

        [HttpGet]
        [Route("returned")]
        public ActionResult<List<ReturnedTransactionDto>> GetReturnedTransactions([FromQuery] int skip = 0, int limit = 5, string search = "")
        {
            var returnedTransactions = _transactionService.GetReturnedTransactions(skip, limit, search);

            return Ok(new
            {
                limit,
                skip,
                total = (search.IsNullOrEmpty() ? _transactionService.CountReturnedTransactions() : _transactionService.CountReturnedSearchTransactions(search)),
                data = returnedTransactions
            });
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

        [HttpPost]
        public ActionResult AddTransaction([FromBody] AddTransactionDto addTransactionDto)
        {
            _transactionService.Add(addTransactionDto);
            return Created();
        }

        [HttpPatch]
        [Route("{transactionId}")]
        public IActionResult UpdateStatus(string transactionId, [FromBody] UpdateStatusTransactionDto updateStatusTransactionDto)
        {
            if (transactionId != updateStatusTransactionDto.Id)
            {
                return BadRequest(new {Message = "Invalid Request"});
            }
            _transactionService.UpdateStatus(transactionId);

            return NoContent();
        }
    }
}
