using Backend.Data;
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
        private readonly ApplicationDbContext _context;

        public TransactionController(ITransactionService transactionService, ApplicationDbContext context)
        {
            _transactionService = transactionService;
            _context = context;
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
        [Route("test-data")]
        public IActionResult GetTestData()
        {
            //var data = (from trans in _context.Transactions where trans.Status == "Returned" select new
            //{
            //    TransactionId = trans.Id
            //} ).ToList();

            var data = (
                from m in _context.Members
                join t in _context.Transactions on m.Id equals t.MemberId
                where t.Status == "Returned"
                group t by new {m.Id, m.Name, m.Gender} into g
                select new
                {
                    MemberId = g.Key.Id,
                    Name = g.Key.Name,
                    Gender = g.Key.Gender,
                    TotalReturnedTransactions = g.Count()
                }
                //select new
                //{
                //    MemberId = m.Id,
                //    MemberName = m.Name,
                //    TransactionId = t.Id,
                //    BorrowDate = t.BorrowDate
                //}
                        )
                        .ToList();


            return Ok(data);
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
