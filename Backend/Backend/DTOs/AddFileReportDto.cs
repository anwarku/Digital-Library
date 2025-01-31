using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class AddFileReportDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public DateOnly ReportDate { get; set; }
    }
}
