using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Transaction
    {
        [Key]
        public string Id { get; set; }
        public DateOnly BorrowDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public string Status { get; set; }
        public int MemberId {  get; set; }

        //public ICollection<DetailTransaction> DetailTransactions { get; set; }
    }
}
