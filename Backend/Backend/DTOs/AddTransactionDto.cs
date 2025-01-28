using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class AddTransactionDto
    {
        public int MemberId { get; set; }
        public List<string> Books { get; set; }
    }
}
