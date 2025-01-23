namespace Backend.DTOs
{
    public class DetailTransactionDto
    {
        public string TransactionId { get; set; }

        public string BookCode { get; set; }

        public BookTransactionDto Books { get; set; }
    }
}
