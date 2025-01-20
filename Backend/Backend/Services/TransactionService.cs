using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Transaction> GetAllTransactions()
        {
            //var transactions = (from a in _context.Transactions join b in _context.DetailTransactions
            //                   on a.Id equals b.TrancationId
            //                    select new
            //                    {
            //                        TransactionId = a.Id,
            //                        DetailTransaction = b.TrancationId
            //                    }).ToList();
            var transactions = _context.Transactions
            .Join(_context.DetailTransactions,
                  a => a.Id,
                  b => b.TransactionId,
                  (a, b) => new
                  {
                      Id = a.Id, // Alias b.TransactionId as Id
                      //TransactionDate = a.TransactionDate, // Include other properties as needed
                      DetailTransactionId = b.TransactionId,
                      //BookId = b.BookId // Include other properties as needed
                  })
            .ToList();
            return _context.Transactions.ToList();
        }

        public Transaction GetTransactionById(string transactionId) 
        {
            return _context.Transactions.FirstOrDefault(t => t.Id == transactionId);
        }

        public Transaction GetLastTransaction()
        {
            var transaction = _context.Transactions
                .FromSql($"SELECT TOP 1 * FROM Transactions ORDER BY Id DESC")
                .First();
            return transaction;
        }

        public void Add(AddTransactionDto addTransactionDto)
        {
            var transaction = new Transaction
            {
            };
        }
    }
}
