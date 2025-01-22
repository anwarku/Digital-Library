using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class DetailTransaction
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        //public Transaction Transaction { get; set; }

        public string BookCode { get; set; }
        //public Book Book { get; set; }
        [NotMapped]
        public List<Book> Books { get; set; }
    }
}
