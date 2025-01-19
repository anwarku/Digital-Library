using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class AddMemberDto
    {
        [Required(ErrorMessage ="Nama anggota wajib diisi!")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Jenis kelamin wajib diisi!")]
        public string Gender { get; set; }

        [Required(ErrorMessage ="Nomor HP wajib diisi!")]
        public string Phone { get; set; }
        public string Job { get; set; }
    }
}
