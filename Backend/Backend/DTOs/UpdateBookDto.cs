using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class UpdateBookDto
    {
        [Required(ErrorMessage = "Kode buku harus diisi!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Stok buku harus diisi!")]
        [Range(0, 50, ErrorMessage = "Stok buku antara 0 sampai 50")]
        public int Stock { get; set; }
    }
}
