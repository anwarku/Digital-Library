using Backend.DTOs;

namespace Backend.Services.Interfaces
{
    public interface ITransactionService
    {
        List<TransactionDto> GetAllTransactions();
        List<BorrowedTransactionDto> GetBorrowedTransactions(int skip, int limit, string search);
        List<ReturnedTransactionDto> GetReturnedTransactions(int skip, int limit, string search);
        TransactionDto GetTransactionById(string transactionId);
        TransactionDto GetLastTransaction();
        void Add(AddTransactionDto addTransactionDto);
        void UpdateStatus(string transactionId);
        int CountBorrowedTransactions();
        int CountReturnedTransactions();
        int CountBorrowedSearchTransactions(string search);
        int CountReturnedSearchTransactions(string search);
    }
}
