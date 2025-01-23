using System.ComponentModel.DataAnnotations.Schema;
using Backend.DTOs;

namespace Backend.Models
{
    public class DetailTransaction
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        //public Transaction Transaction { get; set; }

        public string BookCode { get; set; }
        //public Book Book { get; set; }
        
    }
}
