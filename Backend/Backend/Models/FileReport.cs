namespace Backend.Models
{
    public class FileReport
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string TypeFile { get; set; }
        public Guid UserId { get; set; }
        public DateOnly ReportDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
