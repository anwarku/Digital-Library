namespace Backend.DTOs
{
    public class BorrowedTransactionDto
    {
        public string Id { get; set; }
        public DateOnly BorrowDate { get; set; }
        public string Status { get; set; }
    }
}
