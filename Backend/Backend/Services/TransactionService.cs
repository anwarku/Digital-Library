using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.Helper;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public TransactionService(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
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

        public List<BorrowedTransactionDto> GetBorrowedTransactions(int skip, int limit, string search)
        {
            var borrowedTransactionDto = new List<BorrowedTransactionDto>();
            var borrowedTransaction = _context.Transactions
                .Where(t => t.Status == "Borrowed")
                .Where(t => t.Id.Contains(search))
                .OrderByDescending(t => t.Id)
                .Skip(Math.Abs(skip))
                .Take(limit)
                .ToList();

            foreach (var transaction in borrowedTransaction)
            {
                borrowedTransactionDto.Add(new BorrowedTransactionDto 
                {
                    Id = transaction.Id,
                    MemberName = _context.Members.First(m => m.Id == transaction.MemberId).Name,
                    BorrowDate = transaction.BorrowDate,
                    Status = transaction.Status,
                }
                );
            }

            return borrowedTransactionDto;
        }

        public List<ReturnedTransactionDto> GetReturnedTransactions(int skip, int limit, string search)
        {
            var returnedTransactionDto = new List<ReturnedTransactionDto>();
            var returnedTransaction = _context.Transactions
                .Where(t => t.Status == "Returned")
                .Where(t => t.Id.Contains(search))
                .OrderByDescending(t => t.Id)
                .Skip(Math.Abs(skip))
                .Take(limit)
                .ToList();

            foreach (var transaction in returnedTransaction)
            {
                returnedTransactionDto.Add(new ReturnedTransactionDto
                {
                    Id = transaction.Id,
                    MemberName = _context.Members.First(m => m.Id == transaction.MemberId).Name,
                    BorrowDate = transaction.BorrowDate,
                    ReturnDate = transaction.ReturnDate.HasValue ? transaction.ReturnDate.Value : default(DateOnly),
                    Status = transaction.Status,
                }
                );
            }

            return returnedTransactionDto;
        }

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
                MemberName = _context.Members.First(m => m.Id == transaction.MemberId).Name,
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
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                string newTransactionId = GenerateTransactionId();

                var newTransaction = new Transaction
                {
                    Id = newTransactionId,
                    BorrowDate = DateOnly.FromDateTime(DateTime.Now),
                    Status = "Borrowed",
                    MemberId = addTransactionDto.MemberId
                };

                _context.Transactions.Add(newTransaction);
                _context.SaveChanges();

                foreach (var bookCode in addTransactionDto.Books)
                {
                    var newDetailTransaction = new DetailTransaction
                    {
                        BookCode = bookCode,
                        TransactionId = newTransactionId
                    };

                    _context.DetailTransactions.Add(newDetailTransaction);
                    _context.SaveChanges();
                }

                transaction.Commit();

            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }

        }

        public void UpdateStatus(string transactionId)
        {
            var existTransaction = _context.Transactions.Find(transactionId);
            if (existTransaction != null && existTransaction.Status == "Borrowed") 
            {
                existTransaction.Status = "Returned";
                existTransaction.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
                _context.SaveChanges();
            }
        }

        public int CountBorrowedTransactions()
        {
            return _context.Transactions
                .Where(t => t.Status == "Borrowed")
                .Count(); 
        }

        public int CountReturnedTransactions()
        { 
            return _context.Transactions
                .Where(t => t.Status == "Returned")
                .Count();
        }

        public int CountBorrowedSearchTransactions(string search)
        {
            var data = _context.Transactions
                .Where(t => t.Id.Contains(search) && t.Status == "Borrowed")
                .ToList();

            return data.Count();
        }

        public int CountReturnedSearchTransactions(string search)
        {
            var data = _context.Transactions
                .Where(t => t.Id.Contains(search) && t.Status == "Returned")
                .ToList();
            return data.Count();
        }

        private string GenerateTransactionId()
        {
            var lastTransaction = _context.Transactions
                .FromSql($"SELECT TOP 1 * FROM Transactions ORDER BY Id DESC")
                .First();
            var lastId = lastTransaction.Id;
            var lastNumber = int.Parse(lastId.Split("-")[1]);
            var newNumber = lastNumber + 1;
            var newId = $"2025-{newNumber.ToString("D4")}";
            return newId;
        }
    }
}
