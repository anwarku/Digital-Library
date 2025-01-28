namespace Backend.DTOs
{
    public class ReturnedTransactionDto
    {

        public string Id { get; set; }
        public string MemberName { get; set; }
        public DateOnly BorrowDate { get; set; }
        public DateOnly ReturnDate { get; set; }
        public string Status { get; set; }
    }
}
