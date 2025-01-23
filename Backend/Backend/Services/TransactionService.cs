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

        public List<TransactionDto> GetAllTransactions()
        {
            // Mendefinisikan list transaction dto untuk menyimpan all transaction
            var allTransactions = new List<TransactionDto>();

            foreach (var transaction in _context.Transactions.ToList())
            {
                var detailTransactions = _context.DetailTransactions.Where(dt => dt.TransactionId == transaction.Id).ToList();
                var dtoDetailTrans = new List<DetailTransactionDto>();

                foreach (var dt in  detailTransactions)
                {
                    dtoDetailTrans.Add(new DetailTransactionDto
                    {
                        BookCode = dt.BookCode,
                        TransactionId = dt.TransactionId,
                        Books = _context.Books.Where(b => b.Code == dt.BookCode).Select(b => new BookTransactionDto { Code = b.Code, Title = b.Title }).First()
                    });
                }

                allTransactions.Add(new TransactionDto { Id = transaction.Id, BorrowDate = transaction.BorrowDate, ReturnDate = transaction.ReturnDate, Status = transaction.Status, detailTransactions = dtoDetailTrans });
            }

            return allTransactions;
        }

        //public List<BorrowedTransactionDto> GetBorrowedTransactions()
        //{
        //    var borrowedTransaction = _context.Transactions.Where(t => t.Status == "Borrowed");
        //}

        public TransactionDto GetTransactionById(string transactionId) 
        {
            var transaction = _context.Transactions.Find(transactionId);

            if (transaction == null)
            {
                throw new Exception("Transaction is not found!");
            }

            var detailTransactions = _context.DetailTransactions.Where(dt => dt.TransactionId == transactionId).ToList();
            var dtoDetailTrans = new List<DetailTransactionDto>();
            foreach (var item in detailTransactions)
            {
                dtoDetailTrans.Add(new DetailTransactionDto
                {
                    TransactionId = item.TransactionId,
                    BookCode = item.BookCode,
                    Books = _context.Books.Where(b => b.Code == item.BookCode).Select(b => new BookTransactionDto { Code = b.Code, Title = b.Title}).First(),
                });
            }

            var resultTransaction = new TransactionDto
            {
                Id = transaction.Id,
                BorrowDate = transaction.BorrowDate,
                ReturnDate = transaction.ReturnDate,
                Status = transaction.Status,
                detailTransactions = dtoDetailTrans
            };

            return resultTransaction;
        }

        public TransactionDto GetLastTransaction()
        {
            var data = _context.Transactions.Where(t => t.Id == "2025-0004").First();
            var detailTransactions = _context.DetailTransactions.Where(dt => dt.TransactionId == "2025-0004").ToList();

            var dtoDetailTrans = new List<DetailTransactionDto>();

            foreach (var item in detailTransactions)
            {
                dtoDetailTrans.Add( new DetailTransactionDto{
                    TransactionId = item.TransactionId,
                    BookCode = item.BookCode,
                    Books = _context.Books.Where(b => b.Code == item.BookCode).Select(b => new BookTransactionDto {Code = b.Code, Title = b.Title }).First(),
                });
            }

            var lastTransaction = new TransactionDto{
                Id = data.Id,
                BorrowDate = data.BorrowDate,
                ReturnDate = data.ReturnDate,
                detailTransactions = dtoDetailTrans
            };
            
            var transaction = _context.Transactions
                .FromSql($"SELECT TOP 1 * FROM Transactions ORDER BY Id DESC")
                .First();
            return lastTransaction;
        }

        public void Add(AddTransactionDto addTransactionDto)
        {
            var transaction = new Transaction
            {
            };
        }
    }
}
