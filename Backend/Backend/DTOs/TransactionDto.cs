using Backend.Models;

namespace Backend.DTOs
{
    public class TransactionDto
    {
        public string Id { get; set; }
        public DateOnly BorrowDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public string Status { get; set; }
        public List<DetailTransactionDto> detailTransactions { get; set; }
    }
}
