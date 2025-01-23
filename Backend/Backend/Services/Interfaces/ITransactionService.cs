using Backend.DTOs;
using Backend.Models;

namespace Backend.Services.Interfaces
{
    public interface ITransactionService
    {
        List<TransactionDto> GetAllTransactions();
        //BorrowedTransactionDto GetBorrowedTransactions();
        TransactionDto GetTransactionById(string transactionId);
        TransactionDto GetLastTransaction();
        void Add(AddTransactionDto addTransactionDto);
    }
}
