using Backend.DTOs;
using Backend.Models;

namespace Backend.Services.Interfaces
{
    public interface ITransactionService
    {
        List<Transaction> GetAllTransactions();
        Transaction GetTransactionById(string transactionId);
        TransactionDto GetLastTransaction();
        void Add(AddTransactionDto addTransactionDto);
    }
}
