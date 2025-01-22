using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class AddBookDto
    {
        [Required(ErrorMessage ="Judul buku harus diisi!")]
        [MinLength(10, ErrorMessage = "Judul buku minimal 10 karakter")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Penulis buku harus diisi!")]
        public string Author { get; set; }

        public string Publisher { get; set; }

        [Required(ErrorMessage = "Tahun terbit buku harus diisi!")]
        [Range(1990, 2025, ErrorMessage ="Tahun terbit antara tahun 1990 sampai 2025")]
        public int PublishYear { get; set; }

        //[MinLength(10, ErrorMessage = "ISBN minimal 10 karakter")]
        public string Isbn { get; set; }

        [Required(ErrorMessage = "Stok buku harus diisi!")]
        [Range(0, 50, ErrorMessage = "Stok buku antara 0 sampai 50")]
        public int Stock { get; set; }
    }
}
