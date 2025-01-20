using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class AddTransactionDto
    {
        [Required(ErrorMessage ="Tanggal bawa harus diisi!")]
        [DataType(DataType.Date, ErrorMessage ="Pastikan berupa tanggal")]
        public DateOnly BorrowDate { get; set; }
        [Required(ErrorMessage ="Member ID harus diisi!")]
        public int MemberId { get; set; }
        [Required]
        public List<string> BookCodes { get; set; }
    }
}
