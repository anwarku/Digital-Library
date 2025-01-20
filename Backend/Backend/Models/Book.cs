using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Book
    {
        [Key]
        public string Code { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int PublishYear { get; set; }
        public string Isbn { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<DetailTransaction> DetailTransactions { get; set; }
    }
}
